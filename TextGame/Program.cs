using TextGame.Entities;
using TextGame.Game;
using TextGame.Utilites;
using TextGame.Data;

namespace TextGame
{
  class Program
  {
    static void Main(string[] args)
    {
      ArgumentNullException.ThrowIfNull(args);

      Console.Write("Please input your characters name: ");

      var player = new Player(Console.ReadLine() ?? "Hero");
      if (player.Name == "") player.Name = "Hero";

      Globals.Initialize(player);

      player.Init();

      Console.WriteLine("===== Player Attacks =====");
      foreach (Attack att in player.Attacks) Console.WriteLine($"Category: {att.Type}, Attack: {att.Name}");
      Console.WriteLine();

      Console.WriteLine("========== Menus =========");
      foreach (var menu in Globals.Menus) Console.WriteLine($"Menu: {menu.Name}");
      Console.WriteLine();

      Console.WriteLine("======= Encounters =======");
      foreach (Encounter encounter in Globals.Encounters)  Console.WriteLine($"Category: {encounter.Type}, Encounter: {encounter.Name}");
      Console.WriteLine();

      Console.WriteLine("========= Attacks ========");
      foreach (Attack attack in Globals.Attacks) Console.WriteLine($"Category: {attack.Type}, Attack: {attack.Name}");
      Console.WriteLine();

      Console.WriteLine("========= Enemies ========");
      foreach (EnemyData enemy in Globals.Enemies) Console.WriteLine($"Category: {enemy.Type}, Enemy: {enemy.Name}");
      Console.WriteLine();

      List<string> effectsList = [];
      Console.WriteLine("========= Effects ========");
      foreach (Effect effect in Globals.Effects) effectsList.Add(effect.Name);
      Console.WriteLine($"Effect: {string.Join(", ", effectsList)}");
      Console.WriteLine();

      Console.WriteLine("====== Armour Items ======");
      foreach (ArmourItem armour in Globals.ArmourItems) Console.WriteLine($"Category: {armour.Slot}, Type: {armour.Type}, Armour: {armour.Name}");
      Console.WriteLine();

      Console.WriteLine("====== Damage Types ======");
      foreach (DamageType DType in Globals.DTypes) Console.WriteLine($"Damage Type: {DType.Name}");
      Console.WriteLine();

      Console.WriteLine("========= Enchants ========");
      foreach (Enchant enchant in Globals.Enchants) Console.WriteLine($"Category: {enchant.Type}, Enchant: {enchant.Name}, Effect: {enchant.Effect.Value}");
      Console.WriteLine();

      Console.WriteLine("====== General Items =====");
      foreach (var item in Globals.Items) Console.WriteLine($"Category: {item.Type}, Item: {item.Name}");
      Console.WriteLine();

      RegisterActions.RegisterAll();

      Console.WriteLine("Welcome to the Text-Based RPG!");
      Console.Write("Press any to continue...");
      Console.ReadLine();

      Console.SetWindowSize(100, 100);

      Settings.Initialize((Console.WindowWidth, Console.WindowHeight));

      GameManager.Start();
    }
  }
}

