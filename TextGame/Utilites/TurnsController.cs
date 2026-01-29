using TextGame.Data;
using TextGame.Entities;
using TextGame.Game;

namespace TextGame.Utilites
{
  internal class TurnsController()
  {
    private static Player P => Globals.Player;
    private static BattleManager Battle => Globals.Battle;
    private static List<Enemy> Enemies => Globals.ActiveEnemies;
    private bool PlayerTurn = true;

    public void StartTurns()
    {
      while (P.IsAlive && Enemies.Any(enemy => enemy.IsAlive))
      {
        if (PlayerTurn)
        {
          Console.WriteLine("Player's Turn:");
          Console.ReadLine();
          PlayerTurn = !PlayerTurn;
          // Player takes action
          MenuManager.ShowMenu("Main");
        }
        else
        {
          Console.WriteLine("Enemies' Turn:");
          Console.ReadLine();
          PlayerTurn = !PlayerTurn;
          // Enemies take action
          EnemyAI.RunEnemyTurn();
          Console.WriteLine("End Enemies' Turn...\nStatus Effects Taking Effect...");
          P.EffectsAffect();
        }
        // Switch turns
      }
      if (P.IsAlive)
      {
        Console.WriteLine("Player wins!");
      }
      else
      {
        Console.WriteLine("Player has been defeated.");
      }
      // Reset battle state
      BattleManager.Reset();
    }
  }
}
