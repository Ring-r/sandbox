using System;
using System.Drawing;
using System.Collections.Generic;

namespace Particles
{
    public class World
    {
        private const int size = 1000;
        private const int size_2 = size >> 1;

        private const int particlesCount_i = 100;
        private const int particlesCount_j = 100;
        private const int particlesCount = particlesCount_i * particlesCount_j;
        Particle[] particles = new Particle[particlesCount];

        private const int stepBetweenParticles = 10;

        public void Create()
        {
            float mi = 0.5f * particlesCount_i * stepBetweenParticles;
            float mj = 0.5f * particlesCount_j * stepBetweenParticles;
            for (int i = 0; i < particlesCount_i; ++i)
            {
                for (int j = 0; j < particlesCount_j; ++j)
                {
                    int k = i * particlesCount_i + j;
                    this.particles[k] = new Particle();
                    this.particles[k].prevPosition.x = i * stepBetweenParticles - mi;
                    this.particles[k].prevPosition.y = j * stepBetweenParticles - mj;
                    this.particles[k].prevPosition.z = Helper.Random.Next(-size_2, size_2);

                    this.particles[k].position.x = this.particles[k].prevPosition.x;
                    this.particles[k].position.y = this.particles[k].prevPosition.y;
                    this.particles[k].position.z = this.particles[k].prevPosition.z;

                    //this.particles[k].radius = step>>1;
                }
            }
        }

        public void Update(float dt)
        {
            for (int i = 0; i < particlesCount; ++i)
            {
                this.particles[i].Move(dt);
            }

            #region Temp code.

            float mi = 0.5f * particlesCount_i * stepBetweenParticles;
            float mj = 0.5f * particlesCount_j * stepBetweenParticles;
            int s = 20;
            for (int i = 0; i < particlesCount_i; i += s)
            {
                int j, k;

                j = 0;
                k = i * particlesCount_i + j;
                this.particles[k] = new Particle();
                this.particles[k].position.x = i * stepBetweenParticles - mi;
                this.particles[k].position.y = j * stepBetweenParticles - mj;
                this.particles[k].position.z = 0;

                j = particlesCount_j - 1;
                k = i * particlesCount_i + j;
                this.particles[k] = new Particle();
                this.particles[k].position.x = i * stepBetweenParticles - mi;
                this.particles[k].position.y = j * stepBetweenParticles - mj;
                this.particles[k].position.z = 0;
            }
            for (int j = 0; j < particlesCount_j; j += s)
            {
                int i, k;

                i = 0;
                k = i * particlesCount_i + j;
                this.particles[k] = new Particle();
                this.particles[k].position.x = i * stepBetweenParticles - mi;
                this.particles[k].position.y = j * stepBetweenParticles - mj;
                this.particles[k].position.z = 0;

                i = particlesCount_i - 1;
                k = i * particlesCount_i + j;
                this.particles[k] = new Particle();
                this.particles[k].position.x = i * stepBetweenParticles - mi;
                this.particles[k].position.y = j * stepBetweenParticles - mj;
                this.particles[k].position.z = 0;
            }

            #endregion Temp code.

            ParticlesLink.UpdateAsCells(this.particles, stepBetweenParticles, 0.5f);

            //ParticlesCollision.Update(this.particles);

        }

        private void DrawParticlesLinksAsLines(Graphics g)
        {
            int k = 0;
            for (int i = 0; i < particlesCount_i; ++i)
            {
                for (int j = 0; j < particlesCount_j - 1; ++j)
                {
                    g.DrawLine(Pens.Black, this.particles[k].position.x, this.particles[k].position.y, this.particles[k + 1].position.x, this.particles[k + 1].position.y);
                    k = k + 1;
                }
                ++k;
            }

            for (int j = 0; j < particlesCount_j; ++j)
            {
                k = j;
                for (int i = 0; i < particlesCount_i - 1; ++i)
                {
                    g.DrawLine(Pens.Black, this.particles[k].position.x, this.particles[k].position.y, this.particles[k + particlesCount_i].position.x, this.particles[k + particlesCount_i].position.y);
                    k += particlesCount_i;
                }
            }
        }
        private void DrawParticlesLinksAsCurves(Graphics g)
        {
            int k = 0;
            for (int i = 0; i < particlesCount_i; ++i)
            {
                List<PointF> points = new List<PointF>(particlesCount_i);
                for (int j = 0; j < particlesCount_j; ++j)
                {
                    points.Add(new PointF(this.particles[k].position.x, this.particles[k].position.y));
                    k = k + 1;
                }
                g.DrawCurve(Pens.Black, points.ToArray());
            }

            for (int j = 0; j < particlesCount_j; ++j)
            {
                k = j;
                List<PointF> points = new List<PointF>(particlesCount_i);
                for (int i = 0; i < particlesCount_i; ++i)
                {
                    points.Add(new PointF(this.particles[k].position.x, this.particles[k].position.y));
                    k += particlesCount_i;
                }
                g.DrawCurve(Pens.Black, points.ToArray());
            }
        }
        public void Draw(Graphics g)
        {
            for (int i = 0; i < particlesCount; ++i)
            {
                g.DrawEllipse(Pens.Black, this.particles[i].position.x - this.particles[i].radius, this.particles[i].position.y - this.particles[i].radius, 2 * this.particles[i].radius, 2 * this.particles[i].radius);
            }

            //this.DrawParticlesLinksAsLines(g);
            this.DrawParticlesLinksAsCurves(g);
        }
    }
}
