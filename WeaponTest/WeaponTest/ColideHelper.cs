using Entities;

namespace WeaponTest
{
    static class ColideHelper
    {
        public static void Check(Evil evil, Weapon weapon)
        {
            for (int i = 0; i < evil.enemies.Count; ++i)
            {
                for (int j = 0; j < weapon.bullets.Count; ++j)
                {
                    if (Check(evil.enemies[i], weapon.bullets[j]))
                    {
                        float enemy_health = evil.enemies[i].Health;
                        evil.enemies[i].Health -= weapon.bullets[j].Health;
                        weapon.bullets[j].Health -= enemy_health;
                    }
                }

            }
        }

        private static bool Check(IEntity enemy, IEntity bullet)
        {
            bool isNotColide =
                (enemy.X + enemy.Width <= bullet.X ||
                bullet.X + bullet.Width <= enemy.X ||
                enemy.Y + enemy.Height <= bullet.Y ||
                bullet.Y + bullet.Height <= enemy.Y);
            return !isNotColide;

            //float x = enemy.X - bullet.X;
            //float y = enemy.Y - bullet.Y;
            //float r = enemy.R + bullet.R;
            //return x * x + y * y < r * r;
        }
    }
}
