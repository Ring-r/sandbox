using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                        evil.enemies[i].Health -= weapon.bullets[j].Health;
                        weapon.bullets[j].Health -= evil.enemies[i].Health + weapon.bullets[j].Health;
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
        }
    }
}
