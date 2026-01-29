using TextGame.Data;
using TextGame.Entities;

namespace TextGame.Utilites
{
  internal static class AttackHelper
  {
    private static List<Enemy> CanAttack => Globals.CanAttack;
    private static List<Enemy> ActiveEnemies => Globals.ActiveEnemies;
    private static string[,] BattleMap => Globals.BattleMap;
    private static Player Player => Globals.Player;

    // ============================
    // ===== Hitable Enemies ======
    // ============================
    public static void Hitable((int x, int y) selfPos)
    {
      CanAttack.Clear();
      foreach (Enemy enemy in ActiveEnemies)
      {
        (int x, int y) enmPos = GetEntityPos(enemy.Name);
        if (CanHit(enmPos, selfPos))
        {
          CanAttack.Add(enemy);
        }
      }
    }

    // ============================
    // ======= ENEMY RANGE ========
    // ============================
    public static bool CanHit((int x, int y) a, (int x, int y) b)
    {
      int dx = Math.Abs(a.x - b.x);
      int dy = Math.Abs(a.y - b.y);

      int dist = Math.Max(dx, dy);
      return dist <= Player.Sight;
    }

    // ============================
    // ===== POSITION LOOKUP ======
    // ============================
    public static (int x, int y) GetEntityPos(string name)
    {
      for (int x = 0; x < BattleMap.GetLength(0); x++) 
        for (int y = 0; y < BattleMap.GetLength(1); y++) 
          if (BattleMap[x, y] == name) return (x, y);
      return (0, 0);
    }

    // ============================
    // ===== MOVE VECTOR ==========
    // ============================
    public static (int x, int y) GetMoveVectorTowardsHero((int x, int y) hero, (int x, int y) target)
    {
      return (Math.Sign(hero.x - target.x),
              Math.Sign(hero.y - target.y));
    }
  }
}
