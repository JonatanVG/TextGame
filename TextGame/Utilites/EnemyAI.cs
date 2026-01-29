using TextGame.Data;
using TextGame.Entities;
using TextGame.Game;

namespace TextGame.Utilites
{
  internal static class EnemyAI
  {
    private static Player P => Globals.Player;
    private static string[,] BMap => Globals.BattleMap;
    private static List<Enemy> Enemies => Globals.ActiveEnemies;
    private static BattleManager Bt => Globals.Battle;
    public static void RunEnemyTurn()
    {
      foreach (var enemy in Enemies)
      {
        // Get positions
        var enemyPos = enemy.Position;
        var heroPos = P.Position;

        if (enemyPos == (0, 0))
        {
          Console.WriteLine($"Error: Enemy '{enemy.Name}' has no position on the map.");
          continue;
        }

        // Check range BEFORE moving
        if (AttackHelper.CanHit(heroPos, enemyPos))
        {
          enemy.AttackTarget();
          Console.WriteLine($"{enemy.Name} attacks {P.Name}!");
          Console.ReadLine();
          continue;
        }

        // Move toward the hero
        var (x, y) = AttackHelper.GetMoveVectorTowardsHero(enemyPos, heroPos);

        var newPos = ( enemyPos.x + x, enemyPos.y + y );

        // Only move if the new tile is empty
        if (newPos.Item1 < 0 || newPos.Item1 >= BMap.GetLength(0) || 
            newPos.Item2 < 0 || newPos.Item2 >= BMap.GetLength(1))
        {
          // New position is out of bounds, skip movement
          continue;
        }
        if (BMap[newPos.Item1, newPos.Item2] == "Empty")
        {
          BMap[enemyPos.x, enemyPos.y] = "Empty";
          BMap[newPos.Item1, newPos.Item2] = enemy.Name;

          // Update enemyPos variable after movement
          enemyPos = newPos;
          Bt.PrintGrid();
        }

        // After moving, check again for attack range
        if (AttackHelper.CanHit(heroPos, enemyPos))
        {
          enemy.AttackTarget();
          Console.WriteLine($"{enemy.Name} attacks {P.Name}!");
          Console.ReadLine();
        }
      }

      // End of enemy turn
    }
  }
}

