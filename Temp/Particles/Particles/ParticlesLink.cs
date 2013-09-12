using System;

namespace Particles
{
    public static class ParticlesLink
    {
        public static void Update(Particle particle_i, Particle particle_j, float defLen, float stiffness)
        {
            Vector midl = (particle_j.position + particle_i.position) * 0.5f;
            Vector norm = (particle_j.position - particle_i.position); norm.Normalize();

            Vector goal1 = midl - norm * defLen * 0.5f;
            Vector goal2 = midl + norm * defLen * 0.5f;

            particle_i.position = particle_i.position + (goal1 - particle_i.position) * stiffness;

            particle_j.position = particle_j.position + (goal2 - particle_j.position) * stiffness;
        }

        public static void UpdateAsCells(Particle[] particles, float defLen, float stiffness)
        {
            int particlesLength = (int)Math.Sqrt(particles.Length);
            int k = 0;
            for (int i = 0; i < particlesLength; ++i)
            {
                for (int j = 0; j < particlesLength - 1; ++j)
                {
                    ParticlesLink.Update(particles[k], particles[k + 1], defLen, stiffness);
                    k = k + 1;
                }
                ++k;
            }

            for (int j = 0; j < particlesLength; ++j)
            {
                k = j;
                for (int i = 0; i < particlesLength - 1; ++i)
                {
                    ParticlesLink.Update(particles[k], particles[k + particlesLength], defLen, stiffness);
                    k += particlesLength;
                }
            }
        }
    }
}
