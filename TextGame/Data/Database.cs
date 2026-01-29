/* This is the main value initializer for the text game. It flattens all the loaded data jsons into a list.
 * This makes the data objects easily accssible and usable all throughout the project. */

using TextGame.Utilites;

namespace TextGame.Data
{
  internal static class Database
  {
    private static bool _isInitialized = false;

    public static List<EnemyData> Enemies { get; private set; } = [];
    public static List<Menu> Menus { get; private set; } = null!;
    public static List<Encounter> Encounters { get; private set; } = [];
    public static EffectData Effects { get; private set; } = null!;
    public static List<Attack> Attacks { get; private set; } = [];
    public static List<ArmourItem> ArmourItems { get; private set; } = [];
    public static List<Item> Items { get; private set; } = [];
    public static DamageTypes DTypes { get; private set; } = [];
    public static List<Enchant> Enchants { get; private set; } = [];

    public static void Initialize()
    {
      if (_isInitialized) return;

      Effects = MenuLoader.LoadEffects();
      DTypes = ItemLoader.LoadDTypes();

      MenuData menuData = MenuLoader.LoadMenus();
      Menus = [.. menuData
        .Values
        .SelectMany(x => x)];

      AttackData attackData = MenuLoader.LoadAttacks();
      Attacks = [.. attackData
        .Values
        .SelectMany(x => x)];

      EnemiesData enemyData = EnemyLoader.LoadEnemies();
      Enemies = [.. enemyData
        .Values
        .SelectMany(x => x)];

      EncounterData encounterData = MenuLoader.LoadEncounters();
      Encounters = [.. encounterData
        .Values
        .SelectMany(x => x)];

      ArmourData armourData = ItemLoader.LoadArmour();
      ArmourItems = [.. armourData
        .Values
        .SelectMany(x => x.Values)
        .SelectMany(y => y)];

      ItemData itemData = ItemLoader.LoadItems();
      Items = [.. itemData
        .Values
        .SelectMany(x => x)];

      LEnchants enchantData = ItemLoader.LoadEnchants();
      Enchants = [.. enchantData
        .Values
        .SelectMany(x => x)];

      _isInitialized = true;
    }

    public static Effect? GetEffect(string name)
    {
      return Effects.FirstOrDefault(x => x.Name == name);
    }

    public static Attack? GetAttack(string name)
    {
      return Attacks.FirstOrDefault(x => x.Name == name);
    }

    public static Menu? GetMenu(string name)
    {
      return Menus.FirstOrDefault(x => x.Name == name);
    }

    public static EnemyData? GetEnemyData(string name)
    {
      return Enemies.FirstOrDefault(x => x.Name == name);
    }

    public static Encounter? GetEncounter(string name)
    {
      return Encounters.FirstOrDefault(x => x.Name == name);
    }

    public static ArmourItem? GetArmourItem(string name)
    {
      return ArmourItems.FirstOrDefault(x => x.Name == name);
    }

    public static Item? GetItem(string name)
    {
      return Items.FirstOrDefault(x => x.Name == name);
    }

    public static DamageType? GetDamageType(string name) 
    {
      return DTypes.FirstOrDefault(x => x.Name == name);
    }

    public static Enchant? GetEnchant(string name) 
    {
      return Enchants.FirstOrDefault(x => x.Name == name);
    }
  }
}
