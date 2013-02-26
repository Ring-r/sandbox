using System;
using System.Collections.Generic;
using System.Drawing;

namespace LockBitsTest
{
    class SimpleParticlesWorld
    {
        public static readonly Random Rand = new Random();
        public const int Count = 1000000;
        public static readonly List<SimpleParticle> Particles = new List<SimpleParticle>(Count);
        public static Size Size = new Size(1, 1);

        static SimpleParticlesWorld()
        {
            for (int i = 0; i < Count; i++)
            {
                Particles.Add(new SimpleParticle()
                {
                    x = Rand.Next(Size.Width),
                    y = Rand.Next(Size.Height),
                    vx = 2 * (float)Rand.NextDouble() - 1,
                    vy = 2 * (float)Rand.NextDouble() - 1,
                    c = Rand.Next(),
                });
            }
        }

        public static void Update()
        {
            SimpleParticle particle;
            for (int i = Particles.Count - 1; i >= 0; --i)
            {
                particle = Particles[i];
                //foreach (SimpleParticle particle in Particles)
                //{
                particle.x += particle.vx;
                particle.y += particle.vy;

                if (particle.x < 0)
                {
                    particle.x = 0;
                    particle.vx = Math.Abs(particle.vx);
                }
                else if (Size.Width <= particle.x)
                {
                    particle.x = Size.Width - 1;
                    particle.vx = -Math.Abs(particle.vx);
                }
                if (particle.y < 0)
                {
                    particle.y = 0;
                    particle.vy = Math.Abs(particle.vy);
                }
                else if (Size.Height <= particle.y)
                {
                    particle.y = Size.Height - 1;
                    particle.vy = -Math.Abs(particle.vy);
                }
            }
        }

        public static void ActionIn(int eX, int eY)
        {
            foreach (SimpleParticle particle in Particles)
            {
                float x = particle.x - eX;
                float y = particle.y - eY;
                float distance = (float)Math.Sqrt(x * x + y * y);
                if (distance == 0)
                {
                    distance = 1;
                }
                float speed = -(10 * (float)Rand.NextDouble() + 1) / distance;
                particle.vx = speed * x;
                particle.vy = speed * y;
            }
        }
    }
}
