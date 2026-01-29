using System.Text.Json;

namespace TextGame.Utilites
{
  internal static class EnemyLoader
  {
    public static EnemiesData LoadEnemies()
    {
      string json = File.ReadAllText("../../../Data/Enemies.json");
      var enemies = JsonSerializer.Deserialize<EnemiesData>(json)!;
      return enemies;
    }
  }
  internal static class ItemLoader
  {
    public static ArmourData LoadArmour()
    {
      string json = File.ReadAllText("../../../Data/Armour.json");
      var armours = JsonSerializer.Deserialize<ArmourData>(json)!;
      return armours;
    }

    public static ItemData LoadItems()
    {
      string json = File.ReadAllText("../../../Data/Items.json");
      var items = JsonSerializer.Deserialize<ItemData>(json)!;
      return items;
    }

    public static DamageTypes LoadDTypes() 
    {
      string json = File.ReadAllText("../../../Data/DamageTypes.json");
      var DTypes = JsonSerializer.Deserialize<DamageTypes>(json)!;
      return DTypes;
    }

    public static LEnchants LoadEnchants()
    {
      string json = File.ReadAllText("../../../Data/Enchants.json");
      var Enchants = JsonSerializer.Deserialize<LEnchants>(json)!;
      return Enchants;
    }
  }
  internal static class MenuLoader
  {
    public static MenuData LoadMenus()
    {
      string json = File.ReadAllText("../../../Data/MainMenu.json");
      var menuData = JsonSerializer.Deserialize<MenuData>(json)!;
      return menuData;
    }

    public static MenuData LoadEncountersMenus()
    {
      string json = File.ReadAllText("../../../Data/EncountersData.json");
      var encounterMenuData = JsonSerializer.Deserialize<MenuData>(json)!;
      return encounterMenuData;
    }

    public static EncounterData LoadEncounters()
    {
      string json = File.ReadAllText("../../../Data/EncountersData.json");
      var encountersData = JsonSerializer.Deserialize<EncounterData>(json)!;
      return encountersData;
    }

    public static AttackData LoadAttacks()
    {
      string json = File.ReadAllText("../../../data/Attacks.json");
      var attackData = JsonSerializer.Deserialize<AttackData>(json)!;
      return attackData;
    }
    public static EffectData LoadEffects()
    {
      string json = File.ReadAllText("../../../data/Effects.json");
      var effectsData = JsonSerializer.Deserialize<EffectData>(json)!;
      return effectsData;
    }
  }

  public sealed record MenuOption(
    string Choice,
    string Outcome,
    string Action
  );

  public sealed record Menu(
    string Name,
    string Parent,
    string Description,
    List<MenuOption> Options
  ) : IMenuLike;

  public class MenuData : Dictionary<string, List<Menu>>
  {
  }

  public class EncounterData : Dictionary<string, List<Encounter>>
  {
  }

  public sealed record EnemyData(
    string Type,
    string Name,
    int Health,
    int Attack,
    int Defense,
    List<string> Attacks,
    List<LootItem>? Drops = null
  );

  public class EnemiesData : Dictionary<string, List<EnemyData>>
  {
  }

  public sealed record Effect(
    string Name,
    int Damage,
    int Duration,
    int MaxStack,
    bool Stun,
    string StackBreak
  )
  {
    public int Duration { get; set; } = Duration;
    public int CurStack { get; set; } = 0;
  }

  public class EffectData: List<Effect>
  {
  }

  public sealed record Attack (
    string Type,
    string Name,
    int Damage,
    int Mana_Cost,
    float Cooldown,
    float Range,
    List<string> Effects,
    string Description
  );

  public class AttackData : Dictionary<string, List<Attack>>
  {
  }

  public sealed record Encounter(
    string Name,
    string Type,
    string Parent,
    string Description,
    List<MenuOption> Options,
    List<string>? Enemies,
    RewardData? Rewards
  ) : IMenuLike;

  public sealed record RewardData(
    int Experience,
    List<LootItem> Loot
  );

  public sealed record LootItem(
    string Item,
    int Quantity
  );

  public sealed record ArmourItem(
    string Name,
    string Description,
    int Intrinsic,
    int Economic,
    string Slot,
    List<string> Covers,
    string Type,
    int Armour,
    List<string> Penalty,
    string Rarity,
    string Quality,
    string Requires,
    List<string> Enchants        
  )
  {
    public int Economic { get; set; } = Economic;
    public List<string> Enchants { get; set; } = Enchants;
  };

  public class ArmourData : Dictionary<string, Dictionary<string, List<ArmourItem>>>
  {
  }

  public sealed record Item
  (
    string Name,
    string Description,
    double Intrinsic,
    double Economic,
    int Quantity,
    int MaxStack,
    string Rarity,
    string Quality,
    string Type,
    List<string> Effect
  )
  {
    public string Name { get; set; } = Name;
    public double Economic { get; set; } = Economic;
    public int Quantity { get; set; } = Quantity;
  }

  public class ItemData : Dictionary<string, List<Item>>
  {
  }

  public sealed record DamageType 
  (
    string Name,
    string Description,
    List<string> Effective,
    List<string> Ineffective
  );

  public class DamageTypes : List<DamageType>
  {
  }

  public sealed record EnEffect
  (
    string Type,
    double Value,
    string? Element = null,
    int? Duration = null,
    int? Interval = null
  );

  public sealed record Enchant
  (
    string Name,
    string Description,
    string Type,
    EnEffect Effect
  );

  public class LEnchants : Dictionary<string, List<Enchant>>
  {
  }

  public interface IMenuLike
  {
    string Parent { get; }
    string Description { get; }
    List<MenuOption> Options { get; }
  };
}
