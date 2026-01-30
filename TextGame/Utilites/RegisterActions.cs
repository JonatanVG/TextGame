using TextGame.Data;
using TextGame.Utilites.Functions;
using TextGame.Entities;
using TextGame.Game;

namespace TextGame.Utilites
{
		internal class RegisterActions
		{
				private static Player P => Globals.Player;
				private static PlayerStats S => Globals.Stats;
    private static string[,] B => Globals.BattleMap;
				private static BattleManager Bt => Globals.Battle;
				private static Attack? currentAttack = P.SelectedAttack;
				private static List<Enemy> Enms => Globals.CanAttack;
				private static List<string> N => Globals.EnemyNames;
				private static TurnsController Turns => Globals.Turns;
				private static BattleManager Battle => Globals.Battle;

    public static void RegisterAll()
				{
						// Example registration of actions
						ActionResolver.Register(@"^attack (\w+)$", (m) =>
						{
								string target = m.Groups[1].Value;
								Console.WriteLine($"Attacking {target}!");
								// Implement attack logic here
						});
						ActionResolver.Register(@"^use (\w+)$", (m) =>
						{
								string item = m.Groups[1].Value;
								Console.WriteLine($"Using {item}!");
								// Implement item usage logic here
						});
						ActionResolver.Register(@"^flee$", (m) =>
						{
								Console.WriteLine("Fleeing from battle!");
								// Implement flee logic here
						});

      // Items and gold
      ActionResolver.Register(@"^Item:(.+):(Add|Remove):(\d+)$", (m) =>
      {
        string itemName = FormatName(m.Groups[1].Value);
								string action = m.Groups[2].Value;
								int quantity = int.Parse(m.Groups[3].Value);

								Item item = Database.GetItem(itemName)!;
				
								if (action == "Add")
								{
										Console.WriteLine($"{itemName}: {quantity}, Action: {action}");
										P.GiveItem(item, quantity);
										Console.ReadLine();
								}
								else
								{
										Console.WriteLine($"{itemName}: {quantity}, Action: {action}");
										P.RemoveItem(item, quantity);
								  Console.ReadLine();
        }
      });

      /// Menus and flow control
      // Menu Changes
      ActionResolver.Register(@"^GoTo:(.+)$", (m) =>
      {
        string target = FormatName(m.Groups[1].Value);
        MenuManager.ShowMenu(target);
      });

						ActionResolver.Register(@"^From:(.+):(.+)$", (m) =>
						{
								string from = FormatName(m.Groups[1].Value);
								string to = FormatName(m.Groups[2].Value);
								MenuManager.ShowMenu(to, from);
						});

      // Exploration
      ActionResolver.Register(@"^Explore$", (m) =>
      {
        Exploration.HandleExploration();
      });

      // Battle
      ActionResolver.Register(@"^Battle:(Ambush)?$", (m) =>
      {
        if (N != null)
        {
          Console.WriteLine(string.Join(", ", N));
          Console.ReadLine();
          Battle.BattleStart(N);
        }
      });

      /// Inventory
      // Inventory Menu
      ActionResolver.Register(@"^Inventory$", (m) =>
						{
								Menu Inventory = Database.GetMenu("Inventory")!;
								P.GetInventory()
										.ForEach(i =>
										{
												if (i.Type == "Consumeable") Inventory.Options.Add(new MenuOption(i.Name, i.Description, $"Use:{i.Name}"));
          });

								MenuManager.ShowMenu("Inventory");
						});

						// Use Item
						ActionResolver.Register(@"^Use:(.+)$", (m) =>
						{
								string itemName = FormatName(m.Groups[1].Value);
								Item? item = P.GetInventoryItem(itemName).FirstOrDefault()!;
								item.Effect.ForEach(eff =>
								{
										ActionResolver.TryExecute(eff);
								});
						});

						// Display stats
						ActionResolver.Register(@"^Stats$", (m) =>
						{
								S.DisplayStats();
      });

      // Unlocks
      ActionResolver.Register(@"^attain(.+)$", (m) =>
      {
        string skill = FormatName(m.Groups[1].Value);
        P.Attacks.Add(Database.GetAttack(skill)!);
      });

						/// Player actions
      // Healing actions
      ActionResolver.Register(@"^Heal:(\d+)$", (m) =>
						{
								int amount = int.Parse(m.Groups[1].Value);
								Console.WriteLine($"Healing for {amount} points!");
								P.Heal(amount);
						});

						// Stat Choice
						ActionResolver.Register(@"^StatChoice$", (m) =>
						{
								Console.WriteLine("Choose a stat to increase:");
								S.DisplayStats();
								Console.Write("Enter stat name: ");
								string? choice = Console.ReadLine();
								if (!string.IsNullOrEmpty(choice)) 
								{
										string stat = $"Stat{choice}";
										S.IncreaseStat([stat], 1);
        }
      });

      // Use item actions
      ActionResolver.Register(@"^Stat:(.+):(Up|Down):(\d+)$", (m) =>
						{
								string stat = FormatName(m.Groups[1].Value);
								string dir  = m.Groups[2].Value;
								int amount  = int.Parse(m.Groups[3].Value);
								List<string> stats = [.. stat.Split(',').Select(s => s.Trim())];
								S.IncreaseStat(stats, dir == "Up" ? amount : -amount);
      });

      // Movement actions
      ActionResolver.Register(@"^Grid:(.+)$", (m) =>
						{
								string dir = m.Groups[1].Value;
								(int x, int y) heroPos = P.Position;

								if (string.IsNullOrEmpty(dir))
								{
										Console.WriteLine($"No action found...");
										return;
								}
								if (B == null)
								{
										Console.WriteLine("BattleMap is null");
										return;
								}

								switch (dir)
								{
										case "Up":
												if (heroPos.y - 1 < 0)
												{
														Console.WriteLine("Cannot move up, out of bounds.");
														Console.ReadLine();
														break;
												}
												B[heroPos.x, heroPos.y] = "Empty";
												B[heroPos.x, heroPos.y - 1] = P.Name;
												Bt.PrintGrid();
												Console.WriteLine("Moved Up\nPress any to continue...");
												Console.ReadLine();
												break;
										case "Down":
												if (heroPos.y + 1 > B.GetLength(1) - 1)
												{
														Console.WriteLine("Cannot move down, out of bounds.");
														Console.ReadLine();
														break;
												}
												B[heroPos.x, heroPos.y] = "Empty";
												B[heroPos.x, heroPos.y + 1] = P.Name;
												Bt.PrintGrid();
												Console.WriteLine("Moved Down\nPress any to continue...");
												Console.ReadLine();
												break;
										case "Left":
												if (heroPos.x - 1 < 0)
												{
														Console.WriteLine("Cannot move left, out of bounds.");
														Console.ReadLine();
														break;
												}
												B[heroPos.x, heroPos.y] = "Empty";
												B[heroPos.x - 1, heroPos.y] = P.Name;
												Bt.PrintGrid();
												Console.WriteLine("Moved Left\nPress any to continue...");
												Console.ReadLine();
												break;
										case "Right":
												if (heroPos.Item1 + 1 > B.GetLength(0) - 1)
												{
														Console.WriteLine("Cannot move right, out of bounds.");
														Console.ReadLine();
														break;
												}
												B[heroPos.x, heroPos.y] = "Empty";
												B[heroPos.x + 1, heroPos.y] = P.Name;
												Bt.PrintGrid();
												Console.WriteLine("Moved Right\nPress any to continue...");
												Console.ReadLine();
												break;
										default:
												break;
								}
								AttackHelper.Hitable(heroPos);
								Bt.RebuildAttackMenu();
								MenuManager.ShowMenu("Main");
						});

						// Attack actions
						ActionResolver.Register(@"^Attack_(.+)$", (m) =>
						{
								string attackName = m.Groups[1].Value;
								Console.WriteLine($"Attack Name: {attackName}");

								var attack = Database.GetAttack(attackName);

								if (attack != null)
								{
										Console.WriteLine($"Attack action: AttackTypes not null: Attack '{attackName}' found!");
										if (attack.Name == "Heal")
										{
												P.Heal(Math.Min(S.MaxHealth - P.Health, attack.Damage));
												MenuManager.ShowMenu("Main");
										}
										currentAttack = attack;
										MenuManager.ShowMenu("Attack Menu", $"{attack.Name}");
								}

								Console.WriteLine($"Attack action: AttackTypes null: Attack '{attackName}' not found!");
								Console.ReadLine();
						});

						ActionResolver.Register(@"Target_(.+)$", (m) =>
						{
								string targetName = m.Groups[1].Value;
								Console.WriteLine($"Target Name: {targetName}");

								if (Enms == null || Enms.Count == 0)
								{
										Console.WriteLine("No enemies are currently active!");
										return;
								}

								foreach (Enemy enm in Enms) Console.WriteLine($"Found enemy '{enm}' in eList at {Enms.IndexOf(enm)}.");

								Enemy? target = Enms.FirstOrDefault(e => e.Name.Equals(targetName, StringComparison.OrdinalIgnoreCase));

								if (target == null)
								{
										Console.WriteLine($"Enemy '{targetName}' not found!");
										return;
								}

								Console.WriteLine($"Enemy '{targetName}' found!");

								if (currentAttack != null)
								{
										Console.WriteLine($"Attack '{currentAttack.Name}' found!");
										target.TakeDamage(currentAttack.Damage);
										Console.WriteLine("Press any to continue...");
										Console.ReadLine();
										Turns.StartTurns();
								}
						});

						// Damage actions
						ActionResolver.Register(@"^Harm:(\d+)$", (m) =>
						{
								int dmg = int.Parse(m.Groups[1].Value);
								P.TakeDamage(dmg);
						});
				}

				private static string FormatName(string str)
				{
						str = str.Trim().Replace("-", " ");
						return char.ToUpper(str[0]) + str[1..];
				}
		}
}
