using System;

namespace Components
{
    public class HealthComponent
    {
        private int minHP;
        private int maxHP;

        private int hp;
        public int HP
        {
            get => hp;
            set
            {
                hp = Math.Clamp(value, minHP, maxHP);
                ChangeHP?.Invoke(hp);
                if (hp < 0)
                {
                    Die?.Invoke();
                }
            }
        }

        public HealthComponent(int HP, int minHP, int maxHP)
        {
            this.minHP = minHP;
            this.maxHP = maxHP;

            this.HP = HP;
        }

        public Action<int> ChangeHP;
        public Action Die;
    }
}