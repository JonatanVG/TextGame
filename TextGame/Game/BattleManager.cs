using TextGame.Data;
using TextGame.Entities;
using TextGame.Utilites;

namespace TextGame.Game
{
  internal class BattleManager()
  {
    private static Player P => Globals.Player;
    private static PlayerStats S => Globals.Stats;
    private static TurnsController Turns => Globals.Turns;
    private static List<string> N => Globals.EnemyNames;
    private static string[,] BMap => Globals.BattleMap;
    private static List<Enemy> ActiveEnemies => Globals.ActiveEnemies;
    private static List<Enemy> CanAttack => Globals.CanAttack;

    // ============================
    // ==== BATTLE START LOGIC ====
    // ============================
    public void BattleStart(List<string> enemyNames)
    {
      GenerateEnemies(enemyNames);

      int width = Math.Max(5, ActiveEnemies.Count * 2);
      int height = width;

      Battle(width, height);
    }

    // ============================
    // ===== GENERATE ENEMIES =====
    // ============================
    private void GenerateEnemies(List<string> enemyNames)
    {
      var nameCounter = new Dictionary<string, int>();

      foreach (string name in enemyNames)
      {
        EnemyData enmDt = Database.GetEnemyData(name)!;
        // count duplicates
        nameCounter.TryGetValue(name, out int count);
        count++;
        nameCounter[name] = count;

        string trimmed = name.Length > 8 ? name[..8] : name;
        string finalName = $"{trimmed}-{count}";

        Enemy enemy = new(finalName, enmDt.Health, enmDt.Attack, enmDt.Defense, enmDt.Attacks);
        enemy.OnEnemyDeleted += HandleEnemyDeath;
        ActiveEnemies.Add(enemy);
      }
    }

    // ============================
    // ======= ATTACK MENU ========
    // ============================
    public void RebuildAttackMenu()
    {
      var attackMenu = Database.GetMenu("Attack Menu")!;
      attackMenu.Options.Clear();

      foreach (Enemy e in CanAttack) attackMenu.Options.Add(new MenuOption(e.Name, e.Name, $"Target_{e.Name}"));
    }

    // ============================
    // ======== BATTLE MAP ========
    // ============================
    private void Battle(int width, int height)
    {
      Globals.BMapSet(new string[width, height]);

      // fill empty
      for (int x = 0; x < width; x++)
          for (int y = 0; y < height; y++) BMap[x, y] = "Empty";

      // hero center
      P.Position = (width / 2, height / 2);
      BMap[P.Position.Item1, P.Position.Item2] = P.Name;

      AttackHelper.Hitable(P.Position);
      RebuildAttackMenu();

      // enemy placement (even spread)
      PlaceEnemies(width, height);

      // show grid
      PrintGrid();
      Console.ReadLine();

      // exit to menu
      Turns.StartTurns();
    }

    private void PlaceEnemies(int width, int height)
    {
      int ring = 1;
      int idx = 0;

      foreach (var e in ActiveEnemies)
      {
        // basic ring-spawn pattern
        int px = ring;
        int py = idx % (height - 2);

        px = Math.Clamp(px, 0, width - 1);
        py = Math.Clamp(py, 0, height - 1);

        BMap[px, py] = e.Name;
        e.Position = (px, py);

        idx++;
        if (idx % (height - 2) == 0) ring++;
      }
    }

    // ============================
    // ======= ENEMY DEATH ========
    // ============================
    private void HandleEnemyDeath(Enemy enemy)
    {
      enemy.OnEnemyDeleted -= HandleEnemyDeath;
      ActiveEnemies.Remove(enemy);
      CanAttack.Remove(enemy);

      Menu attackMenu = Database.GetMenu("Attack Menu")!;
      MenuOption toRemove = attackMenu.Options.First(o => o.Choice == enemy.Name);
      attackMenu.Options.Remove(toRemove);

      BMap[enemy.Position.x, enemy.Position.y] = "Empty";
    }

    // ============================
    // ====== PRINTING GRID =======
    // ============================
    public void PrintGrid()
    {
      var view = GenerateHeroView();

      int width = view.GetLength(0);
      int height = view.GetLength(1);

      int maxLen = view.Cast<string>().Where(v => v != null).DefaultIfEmpty("").Max(v => v.Length);
      int cellWidth = maxLen + 2;

      PrintBorder(width, cellWidth);

      for (int y = 0; y < height; y++)
      {
        Console.Write("|");
        for (int x = 0; x < width; x++)
        {
          string val = view[x, y] ?? "Empty";

          if (val == P.Name) Console.ForegroundColor = ConsoleColor.Green;
          else if (val.StartsWith("Goblin") || val.StartsWith("Dragon")) Console.ForegroundColor = ConsoleColor.Red;
          else Console.ForegroundColor = ConsoleColor.Gray;

          Console.Write(val.PadRight(cellWidth));
          Console.ResetColor();
          Console.Write("|");
        }
        Console.WriteLine();
        PrintBorder(width, cellWidth);
      }
    }

    private static void PrintBorder(int count, int size)
    {
      for (int i = 0; i < count; i++)
      {
        Console.Write("+");
        Console.Write(new string('-', size));
      }
      Console.WriteLine("+");
    }

    // ============================
    // ===== HERO VIEW GRID =======
    // ============================
    public string[,] GenerateHeroView()
    {
      int hx = P.Position.Item1;
      int hy = P.Position.Item2;
      int sight = P.Sight;
      string[,] bMap = BMap;
      int x0 = Math.Max(0, hx - sight);
      int y0 = Math.Max(0, hy - sight);
      int x1 = Math.Min(bMap.GetLength(0), hx + sight + 1);
      int y1 = Math.Min(bMap.GetLength(1), hy + sight + 1);

      int w = x1 - x0;
      int h = y1 - y0;

      string[,] result = new string[w, h];

      for (int x = x0; x < x1; x++)
        for (int y = y0; y < y1; y++) result[x - x0, y - y0] = bMap[x, y];

      return result;
    }

    // ============================
    // ====== RESET MANAGER =======
    // ============================
    public static void Reset()
    {
      ActiveEnemies.Clear();
      CanAttack.Clear();
      N.Clear();
      Globals.BMapSet(new string[1,1]);
    }
  }
}

