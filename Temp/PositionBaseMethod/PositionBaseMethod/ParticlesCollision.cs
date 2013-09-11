﻿using System;

namespace PositionBasedMethod
{
    public class ParticlesCollision
    {
        public static void Update(Particle[] particles)
        {
            int particlesCount = particles.Length;
            for (int i = 0; i < particlesCount - 1; i++)
            {
                for (int j = i + 1; j < particlesCount; j++)
                {
                    Vector3 p1 = particles[i].position;
                    Vector3 p2 = particles[j].position;
                    Vector3 penetrationDirection = (p2 - p1);
                    float penetrationSquareLength = penetrationDirection.SquareLength();
                    if (penetrationSquareLength < Math.Sqrt(particles[i].radius + particles[j].radius))
                    {
                        penetrationDirection.Normalize();
                        float penetrationLength = (float)Math.Sqrt(penetrationSquareLength);
                        float penetrationDepth = 0.5f * (particles[i].radius + particles[j].radius - penetrationLength);

                        particles[i].position -= penetrationDirection * penetrationDepth;
                        particles[j].position += penetrationDirection * penetrationDepth;
                    }
                }
            }
        }
    }
}
