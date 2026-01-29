using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TextGame.Data;
using TextGame.Utilites;

namespace TextGame.Entities
{
    internal class Player(string name = "Hero", int maxHealth = 50, int attack = 5, int defense = 1, int mana = 20)
    {
        private static readonly List<string> value = ["Punch", "Earth Shatter", "Slash", "Heal"];
        private static readonly List<string> zones = [ "Head", "Neck", "Torso", "Shoulder", "Arms Upper", "Arms Lower", "Hands", "Waist", "Leg Upper", "Leg Lower", "Feet" ];
        private static readonly List<string> armourTypes = [ "Rigid", "Mail", "Padded" ];

        public string Name { get; set; } = name;
        public int MaxHealth { get; private set; } = maxHealth;
        public int Health { get; private set; } = maxHealth;
        public int Strength { get; private set; } = attack;
        public int Defense { get; private set; } = defense;
        public int Mana { get; set; } = mana;
        public int Sight { get; set; } = 9;
        public int Level { get; set; } = 1;
        public (int x, int y) Position { get; set; } = (0, 0);
        public List<Item> Inventory { get; private set; } = [];
        public List<Effect> Conditions { get; private set; } = [];
        public List<Attack> Attacks { get; private set; } = [];
        public List<string> Skills { get; private set; } =
        [
            "attainFireball"
        ];
        public Dictionary<string, Dictionary<string, ArmourItem>> ArmourModel = [];
        public Attack SelectedAttack { get; set; } = null!;

        public bool IsAlive => Health > 0;

        public void Living()
        {
            if (!IsAlive)
            {
                Console.WriteLine("You died...\nPress any to return to main...");
                Console.Read();
            }
        }

        public void TakeDamage(int amount)
        {
            Health = Math.Max(0, Health - Math.Max(1, amount - Defense));
            Console.WriteLine($"{Name} takes {amount} damage! Remaining HP: {Health}");
            Living();
        }

        public void Heal(int amount)
        {
            Health = Math.Min(MaxHealth, Health + amount);
        }

        public void GiveItem(Item item, int count)
        {
            List<Item?> exists = GetInventoryItem(item.Name);

            if (exists.Count > 0)
            {
                exists.ForEach(item =>
                {
                    if (item!.Quantity < item.MaxStack && count > 0)
                    {
                        int spaceInItem = item.MaxStack - item.Quantity;
                        int toAdd = int.Min(spaceInItem, count);

                        item.Quantity += toAdd;
                        count -= toAdd;

                        Console.WriteLine($"Added {toAdd} to existing stack, now has {item.Quantity}");
                    }
                });
            }

            while (count > 0)
            {
                int itemSize = int.Min(item.MaxStack, count);
                Item newItem = Database.GetItem(item.Name)!;

                Inventory.Add(newItem with { Quantity = itemSize });
                count -= itemSize;

                Console.WriteLine($"Created new '{item.Name}' stack with {itemSize} items");
            }
        }

        public void RemoveItem(Item item, int amount)
        {
            List<Item?> exists = GetInventoryItem(item.Name);

            for (int i = exists.Count - 1; i >= 0 && amount > 0; i--)
            {
                Item target = exists[i];
                int toRm = int.Min(target.Quantity, amount);

                item.Quantity -= toRm;
                amount -= toRm;

                Console.WriteLine($"Removed {toRm} from item, now has {target.Quantity}");

                if (target.Quantity < 0) Inventory.Remove(target);
                Console.WriteLine($"Removed empty item of {item.Name}");
            }
        }

        public List<Item?> GetInventoryItem(string name)
        {
            return [.. Inventory.FindAll(x => x.Name.StartsWith(name, StringComparison.OrdinalIgnoreCase))!];
        }

        public void EffectsAffect()
        {
            foreach (Effect effect in Conditions)
            {
                int dam = effect.Damage * (effect.CurStack / effect.CurStack ^ 2);
                Health -= dam;
                Console.WriteLine($"Affected by: {effect.Name} for {dam}");
                effect.Duration--;
                effect.CurStack--;
            }
        }

        public void Init()
        {
            Attacks.AddRange([.. value.Select(x => Database.GetAttack(x)!)]);
            Attacks.ForEach(attack =>
            {
                var menu = Database.GetMenu(attack.Type);
                menu?.Options.Add(new MenuOption(
                    attack.Name,
                    attack.Description,
                    $"Attack_{attack.Name}"
                ));
            });

            ArmourModel = zones.ToDictionary(
                zone => zone,
                zone => armourTypes.ToDictionary(
                    type => type,
                    type => (ArmourItem)null!
                )
            )!;
        }

        public bool EquipArmour(ArmourItem item)
        {
            if (item == null || !ArmourModel.ContainsKey(item.Slot)) return false;

            var slotDict = ArmourModel[item.Slot];
            if (!slotDict.ContainsKey(item.Type)) return false;

            var current = slotDict[item.Type];
            if (current != null) UnequipArmour(current);

            slotDict[item.Type] = item;

            ApplyArmourEffects(item);

            return true;
        }

        public bool UnequipArmour(ArmourItem item)
        {
            if (item == null || !ArmourModel.ContainsKey(item.Slot)) return false;

            var slotDict = ArmourModel[item.Slot];
            if (slotDict[item.Type] == item)
            {
                slotDict[item.Type] = null!;
                RemoveArmourEffects(item);
                return true;
            }

            return false;
        }

        public int GetZoneArmour(string zone)
        {
            if (!ArmourModel.TryGetValue(zone, out Dictionary<string, ArmourItem>? value)) return 0;

            return value.Values
                .Where(item => item != null)
                .Sum(item => item.Armour);
        }

        public List<ArmourItem> GetAllEquippedArmour()
        {
            return [.. ArmourModel.Values
                .SelectMany(typeDict => typeDict.Values)
                .Where(item => item != null)];
        }

        public bool IsSlotArmed(string zone)
        {
            if (!ArmourModel.TryGetValue(zone, out Dictionary<string, ArmourItem>? value)) return false;

            var paddedItem = value["Padded"];
            return paddedItem != null && paddedItem.Enchants?.Contains("Armed") == true;
        }

        private void ApplyArmourEffects(ArmourItem item)
        {
            if (item.Enchants == null) return;

            item.Enchants.ForEach(enchant =>
            {

            });
        }
        private void RemoveArmourEffects(ArmourItem item)
        {
            if (item.Enchants == null) return;

            item.Enchants.ForEach(enchant =>
            {

            });
        }

        public void LevelUp()
        {
            Level++;
            MaxHealth += 10;
            Health = MaxHealth;
            Strength += 2;
            Defense += 1;
            Mana += 5;
            Console.WriteLine($"Leveled up to {Level}! Stats increased.");
        }

        public void DisplayStats()
        {
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Level: {Level}");
            Console.WriteLine($"Health: {Health}/{MaxHealth}");
            Console.WriteLine($"Strength: {Strength}");
            Console.WriteLine($"Defense: {Defense}");
            Console.WriteLine($"Mana: {Mana}");
        }

        public void DisplayInventory()
        {
            Console.WriteLine("Inventory:");
            Inventory.ForEach(item => Console.WriteLine($"- {item.Name}, Quantity: {item.Quantity}"));
        }
        
        public void DisplayArmour()
        {
            Console.WriteLine("Equipped Armour:");
            foreach (var zone in ArmourModel.Keys)
            {
                var typeDict = ArmourModel[zone];
                foreach (var type in typeDict.Keys)
                {
                    var item = typeDict[type];
                    if (item != null)
                    {
                        Console.WriteLine($"- {zone} ({type}): {item.Name} (Armour: {item.Armour})");
                    }
                }
            }
        }

        public void ResetConditions()
        {
            Conditions.Clear();
        }

        public void RestoreMana(int amount)
        {
            Mana += amount;
        }

        public void ConsumeMana(int amount)
        {
            Mana = Math.Max(0, Mana - amount);
        }

        public void ResetPlayer()
        {
            Health = MaxHealth;
            Mana = 20;
            Inventory.Clear();
            Conditions.Clear();
            ArmourModel = zones.ToDictionary(
                zone => zone,
                zone => armourTypes.ToDictionary(
                    type => type,
                    type => (ArmourItem)null!
                )
            )!;
        }

        public void LearnSkill(string skillName)
        {
            if (!Skills.Contains(skillName))
            {
                Skills.Add(skillName);
                Console.WriteLine($"Learned new skill: {skillName}");
            }
        }

        public void ForgetSkill(string skillName)
        {
            if (Skills.Contains(skillName))
            {
                Skills.Remove(skillName);
                Console.WriteLine($"Forgot skill: {skillName}");
            }
        }

        public void DisplaySkills()
        {
            Console.WriteLine("Skills:");
            foreach (string skill in Skills)
            {
                Console.WriteLine($"- {skill}");
            }
        }

        public void ResetSkills()
        {
            Skills.Clear();
        }

        public void DisplayAttacks()
        {
            Console.WriteLine("Attacks:");
            foreach (Attack attack in Attacks)
            {
                Console.WriteLine($"- {attack.Name} (Damage: {attack.Damage}, Mana Cost: {attack.Mana_Cost})");
            }
        }

        public void ResetAttacks()
        {
            Attacks.Clear();
        }

        public void SelectAttack (Attack attack)
        {
            SelectedAttack = attack;
        }

        public void DeselectAttack()
        {
            SelectedAttack = null!;
        }
    }
}

