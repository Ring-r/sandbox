using System;
using System.Drawing;
using System.Collections.Generic;

namespace PositionBasedMethod
{
    public class World
    {
        private const int max = 1000;

        private Random random = new Random();

        private const int particlesLength = 30;
        private const int particlesCount = particlesLength * particlesLength;
        Particle[] particles = new Particle[particlesCount];

        const int step = 30;

        public void Create()
        {
            float m = particlesLength * step / 2;
            for (int i = 0; i < particlesLength; ++i)
            {
                for (int j = 0; j < particlesLength; ++j)
                {
                    int k = j * particlesLength + i;
                    this.particles[k] = new Particle();
                    this.particles[k].prevPosition.x = i * step - m;
                    this.particles[k].prevPosition.y = j * step - m;
                    this.particles[k].prevPosition.z = this.random.Next(-max / 2, max / 2);

                    this.particles[k].position.x = this.particles[k].prevPosition.x;
                    this.particles[k].position.y = this.particles[k].prevPosition.y;
                    this.particles[k].position.z = this.particles[k].prevPosition.z;

                    this.particles[k].acceleration.z = 0.01f;

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

            ParticlesLink particleLink = new ParticlesLink(step, 0.5f);
            int k = 0;
            for (int i = 0; i < particlesLength; ++i)
            {
                for (int j = 0; j < particlesLength - 1; ++j)
                {
                    particleLink.Update(this.particles[k], this.particles[k + 1]);
                    k = k + 1;
                }
                ++k;
            }

            for (int j = 0; j < particlesLength; ++j)
            {
                k = j;
                for (int i = 0; i < particlesLength - 1; ++i)
                {
                    particleLink.Update(this.particles[k], this.particles[k + particlesLength]);
                    k += particlesLength;
                }
            }

            ParticlesCollision.Update(this.particles);
        }

        public void Draw(Graphics g)
        {
            for (int i = 0; i < particlesCount; ++i)
            {
                g.DrawEllipse(Pens.Black, this.particles[i].position.x - this.particles[i].radius, this.particles[i].position.y - this.particles[i].radius, 2 * this.particles[i].radius, 2 * this.particles[i].radius);
            }

            //int k = 0;
            //for (int i = 0; i < particlesLength; ++i)
            //{
            //    for (int j = 0; j < particlesLength - 1; ++j)
            //    {
            //        g.DrawLine(Pens.Black, this.particles[k].position.x, this.particles[k].position.y, this.particles[k + 1].position.x, this.particles[k + 1].position.y);
            //        k = k + 1;
            //    }
            //    ++k;
            //}

            //for (int j = 0; j < particlesLength; ++j)
            //{
            //    k = j;
            //    for (int i = 0; i < particlesLength - 1; ++i)
            //    {
            //        g.DrawLine(Pens.Black, this.particles[k].position.x, this.particles[k].position.y, this.particles[k + particlesLength].position.x, this.particles[k + particlesLength].position.y);
            //        k += particlesLength;
            //    }
            //}



            int k = 0;
            for (int i = 0; i < particlesLength; ++i)
            {
                List<PointF> points = new List<PointF>(particlesLength);
                for (int j = 0; j < particlesLength; ++j)
                {
                    points.Add(new PointF(this.particles[k].position.x, this.particles[k].position.y));
                    k = k + 1;
                }
                g.DrawCurve(Pens.Black, points.ToArray());
            }

            for (int j = 0; j < particlesLength; ++j)
            {
                k = j;
                List<PointF> points = new List<PointF>(particlesLength);
                for (int i = 0; i < particlesLength; ++i)
                {
                    points.Add(new PointF(this.particles[k].position.x, this.particles[k].position.y));
                    k += particlesLength;
                }
                g.DrawCurve(Pens.Black, points.ToArray());
            }
        }
    }
}
