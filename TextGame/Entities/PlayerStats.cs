using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TextGame.Data;
using TextGame.Utilites.Functions;

namespace TextGame.Entities
{
  internal class PlayerStats
  {
    private static Player P => Globals.Player;

    private int StatStrength { get; set; } = 5;
    private int StatDexterity { get; set; } = 5;
    private int StatIntelligence { get; set; } = 5;
    private int StatWisdom { get; set; } = 5;
    private int StatConstitution { get; set; } = 5;
    private int StatCharisma { get; set; } = 5;
    private int StatSpeed { get; set; } = 5;
    private int StatLuck { get; set; } = 5;
    private int StatDefense { get; set; } = 0;
    private int StatMaxHealth { get; set; } = 50;
    private int CritChance { get; set; } = 1;
    

    public int Strength => StatStrength;
    public int Dexterity => StatDexterity;
    public int Intelligence => StatIntelligence;
    public int Wisdom => StatWisdom;
    public int Constitution => StatConstitution;
    public int Charisma => StatCharisma;
    public int Speed => StatSpeed;
    public int Luck => StatLuck;
    public int Defense => StatDefense;
    public int MaxHealth => StatMaxHealth;

    public List<PropertyInfo> PlayerProperty(List<string> properties)
    {
      return [.. properties
        .Select(propName => ObjectPropertiesHandler.GetObjectProperty(this, propName)!)
        .Where(propInfo => propInfo != null)];
    }

    public void IncreaseStat(List<string> statNames, int amount)
    {
      List<PropertyInfo> stat = [];
      if (statNames.Contains("All")) stat = ObjectPropertiesHandler.StatInfo();
      if (stat.Count <= 0) stat = PlayerProperty(statNames);
      stat.ForEach(prop =>
      {
        int currentValue = (int)prop.GetValue(this)!;
        prop.SetValue(this, currentValue + amount);
        Console.WriteLine($"Increased {prop.Name} by {amount}. New value: {currentValue + amount}");
      });
      Console.ReadLine();
    }

    public void DisplayStats()
    {
      Console.WriteLine("=== Player Stats ===");
      Console.WriteLine($"Strength:     {StatStrength}");
      Console.WriteLine($"Dexterity:    {StatDexterity}");
      Console.WriteLine($"Intelligence: {StatIntelligence}");
      Console.WriteLine($"Wisdom:       {StatWisdom}");
      Console.WriteLine($"Constitution: {StatConstitution}");
      Console.WriteLine($"Charisma:     {StatCharisma}");
      Console.WriteLine($"Speed:        {StatSpeed}");
      Console.WriteLine($"Luck:         {StatLuck}");
      Console.WriteLine($"Defense:      {StatDefense}");
      Console.WriteLine($"Max Health:   {StatMaxHealth}");
      Console.WriteLine("====================");
      Console.ReadLine();
    }

    public void LevelUpAssist() 
    {
      StatMaxHealth += 10;
      P.Health = MaxHealth;
      StatStrength += 2;
      StatDefense += 1;
    }
  }
}
