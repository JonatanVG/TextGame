using TextGame.Data;
using TextGame.Utilites.Functions;


namespace TextGame.Utilites
{
  internal static class MenuManager
  {
    public static void ShowMenu(string menuName, string? parent = null)
    {
      IMenuLike? menu = null; // IMenuLike initialization
      // IMenuLike is used to generalize Menu, Combat Menu and Encounter types allowing for all to be handled uniformly.

      menu = Database.GetMenu(menuName);
      menu ??= Database.GetEncounter(menuName)!;
      // Game Menu

      int optionsCount = menu.Options.Count;
      List<string> exits = ["exit", (optionsCount + 1).ToString()];

      Console.Clear();
      Console.WriteLine($"\n=== {menuName} ===");
      Console.WriteLine(menu.Description);
      Console.WriteLine();

      menu.Options.ForEach(option => { Console.WriteLine($"{menu.Options.IndexOf(option) + 1}. {option.Choice}"); });

      string exitType = "Exit";
      if (menuName == "Game Menu") exitType = "Pause";
      Console.WriteLine($"{optionsCount + 1}. {exitType}");
      Console.WriteLine();
      Console.Write("Enter option name or number: ");

      var input = Console.ReadLine()?.Trim().ToLower() ?? "";
      if (int.TryParse(input, out int inputAsNumber)) input = inputAsNumber.ToString();

      if (exits.Contains(input.ToString()))
      {
        if (menuName == "Settings Menu")
        {
          parent ??= "Main Menu";
          ShowMenu(parent);
        }
        else if(menu.Parent == "")
        {
          Console.WriteLine("No Parent, exiting.");
          Console.ReadLine();
          Environment.Exit(0);
        }
        ShowMenu(menu.Parent);
      }
      if (input == "0") Environment.Exit(0);

      MenuOption? selectedSearch = menu.Options.FirstOrDefault(x => x.Choice.Equals(input, StringComparison.OrdinalIgnoreCase)) ?? menu.Options.ElementAt(int.Parse(input) - 1) ?? null;
      if (selectedSearch is null)
      {
        Console.WriteLine("Invalid option. Please try again.");
        return;
      }

      Console.WriteLine($"{selectedSearch.Outcome}");
      ActionHandler.HandleAction(selectedSearch.Action);
    }
  }
}

