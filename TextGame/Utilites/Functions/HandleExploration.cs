using TextGame.Data;

namespace TextGame.Utilites.Functions
{
  internal class Exploration
  {
    private static Random Random => Globals.Random;
    private static List<Encounter> Ex => Globals.Encounters;
    private static List<string> N => Globals.EnemyNames;
    public static void HandleExploration()
    {
      List<string> types = new(["Combat"]);

      int randCategoryPos = Random.Next(types.Count);
      string randCategory = types.ElementAt(randCategoryPos);
      var menu = from enc in Ex where enc.Type == randCategory select enc;
      Console.WriteLine($"Category: {randCategory}");
      int randEncounter = Random.Next(menu.Count());
      Encounter encounter = menu.ElementAt(randEncounter);
      string response = $"Encounter Key: {encounter.Name}\nEncounter Description: {encounter.Description}";

      if (encounter.Enemies != null && encounter.Enemies != N)
      {
        N.AddRange(encounter.Enemies);
        response += $"\nEncounter Enemies: {string.Join(", ", N)}";
      }

      Console.WriteLine(response);
      Console.WriteLine(string.Join(", ", N));
      Console.ReadLine();
      MenuManager.ShowMenu(encounter.Name, null);
    }
  }
}
