using TextGame.Data;
using TextGame.Utilites;

namespace TextGame.Entities
{
    internal class Enemy(string name, int maxHealth, int attack, int defense, List<string> attacks)
    {
        public string Name { get; set; } = name;
        public int MaxHealth { get; private set; } = maxHealth;
        public int Health { get; private set; } = maxHealth;
        public int Attack { get; private set; } = attack;
        public int Defense { get; private set; } = defense;
        public (int x,int y) Position { get; set; } = (0,0);
        private List<Attack> Attacks { get; set; } = attacks
            .Select(attName => Database.GetAttack(attName))
            .Where(attack => attack != null)
            .ToList()!;
        public bool IsAlive => Health > 0;
        public event Action<Enemy>? OnEnemyDeleted;
        private static Random RandomInstance => Globals.Random;
        private static Player P => Globals.Player;

        public void AttackTarget()
        {
            Attack? attack = Attacks.ElementAt(RandomInstance.Next(Attacks.Count));
            if (attack is null)
            {
                Console.WriteLine($"{Name} tries to attack but has no valid attacks!");
                return;
            }
            Console.WriteLine($"{Name} uses {attack.Name}!");
            P.TakeDamage(attack.Damage);
            foreach (string effName in attack.Effects)
            {
                Effect effect = Database.GetEffect(effName)!;
                Effect condition = P.Conditions.FirstOrDefault(x => x.Name == effName)!;
                if (condition != null)
                {
                    if (condition.CurStack >= condition.MaxStack)
                    {
                        P.Conditions.Add(Database.GetEffect(condition.StackBreak)!);
                        P.Conditions.Remove(condition);
                        continue;
                    }
                    condition.CurStack++;
                    condition.Duration = effect.Duration;
                    continue;
                }
                P.Conditions.Add(effect);
            }
        }

        public void TakeDamage(int amount)
        {
            Health = Math.Max(0, Health - Math.Max(0, amount - Defense));
            if (!IsAlive)
            {
                Console.WriteLine($"{Name} is hit by a fatal blow as they topple over dead.");
                OnEnemyDeleted?.Invoke(this);
            }
            else
                Console.WriteLine($"{Name} takes {amount} damage! Remaining HP: {Health}");
        }
    }
}
