using System.Diagnostics.CodeAnalysis;
using TextGame.Entities;
using TextGame.Game;
using TextGame.Utilites;

namespace TextGame.Data
{
  /// So, this the Globals class that holds references to key game data. Originally I didn't have this file nor class.
  /// Now, I would say something like "I just didn't want to use globals.", but that would be a lie.
  /// I just didn't know this was a thing I could do in C# to manage global state in a cleaner way.
  /// But now I do!
  internal static class Globals
  {
    public static Player Player { get; private set; } = null!;
    public static BattleManager Battle { get; private set;  } = null!;
    public static TurnsController Turns { get; private set; } = null!;
    public static Random Random { get; private set; } = null!;
    public static List<string> EnemyNames {  get; private set; } = null!;
    public static string[,] BattleMap { get; set; } = null!;
    public static List<Enemy> CanAttack = [];
    public static List<Enemy> ActiveEnemies = [];

    [MemberNotNull(
      nameof(Player),
      nameof(Battle),
      nameof(Turns),
      nameof(Random),
      nameof(EnemyNames),
      nameof(BattleMap)
      )]

    public static void Initialize(Player player)
    {
      Database.Initialize();

      Player = player ?? throw new ArgumentNullException(nameof(player));

      Battle = new BattleManager();
      Turns = new TurnsController();
      Random = new Random();
      EnemyNames = [];
      BattleMap = new string[1,1];
    }

    public static void Reset()
    {
      Player = null!;
      Battle = null!;
      Turns = null!;
      Random = null!;
      EnemyNames = null!;
      BattleMap = null!;
    }

    public static void BMapSet(string [,] map)
    {
      BattleMap = map;
    }

    public static List<EnemyData> Enemies => Database.Enemies;
    public static List<Menu> Menus => Database.Menus;
    public static List<Encounter> Encounters => Database.Encounters;
    public static EffectData Effects => Database.Effects;
    public static List<Attack> Attacks => Database.Attacks;
    public static List<ArmourItem> ArmourItems => Database.ArmourItems;
    public static List<Item> Items => Database.Items;
    public static DamageTypes DTypes => Database.DTypes;
    public static List<Enchant> Enchants => Database.Enchants;
  }
}
