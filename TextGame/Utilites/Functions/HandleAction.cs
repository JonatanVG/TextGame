using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextGame.Utilites.Functions
{
  internal class ActionHandler
  {
    public static void HandleAction(string action)
    {
      if (string.IsNullOrWhiteSpace(action)) return;

      if (!ActionResolver.TryExecute(action)) Console.WriteLine($"[Unknown action] '{action}'");


      var excluded = new HashSet<string> 
      { 
        "default",
        "Explore",
        "GoTo:Game-Menu",
        "GoTo:Settings-Menu",
        "Inventory",
        "Battle:",
        "Battle:Ambush)" 
      };

      if (!excluded.Contains(action))
      {
        Console.Write("Press any key to continue...");
        Console.ReadLine();
        MenuManager.ShowMenu("Game Menu");
      }
    }
  }
}
