using System;
using System.Collections.Generic;
using System.Drawing;

namespace RazorPainterTest
{
    class ParticlesWorld
    {
        public static readonly Random Rand = new Random();

        public int Count = 0;
        public readonly List<Particle> Particles = new List<Particle>();

        public Size Size = new Size(1, 1);

        public void Init()
        {
            this.Particles.Clear();
            for (int i = 0; i < this.Count; i++)
            {
                this.Particles.Add(new Particle()
                {
                    x = Rand.Next(this.Size.Width),
                    y = Rand.Next(this.Size.Height),
                    vx = 2 * (float)Rand.NextDouble() - 1,
                    vy = 2 * (float)Rand.NextDouble() - 1,
                    c = Rand.Next(),
                });
            }
        }

        public void Update()
        {
            foreach (Particle particle in this.Particles)
            {
                particle.x += particle.vx;
                particle.y += particle.vy;

                if (particle.x < 0)
                {
                    particle.x = 0;
                    particle.vx = Math.Abs(particle.vx);
                }
                else if (this.Size.Width <= particle.x)
                {
                    particle.x = this.Size.Width - 1;
                    particle.vx = -Math.Abs(particle.vx);
                }
                if (particle.y < 0)
                {
                    particle.y = 0;
                    particle.vy = Math.Abs(particle.vy);
                }
                else if (this.Size.Height <= particle.y)
                {
                    particle.y = this.Size.Height - 1;
                    particle.vy = -Math.Abs(particle.vy);
                }
            }
        }

        public void ActionIn(int eX, int eY)
        {
            foreach (Particle particle in this.Particles)
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
