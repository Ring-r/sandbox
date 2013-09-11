namespace PositionBasedMethod
{
    public class ParticlesLink
    {
        public float defLen; //длина пружины в ненапряжённом состоянии
        public float stiffness; //параметр, задающий жёсткость связи

        public ParticlesLink(float defLen, float stiffness)
        {
            this.defLen = defLen;
            this.stiffness = stiffness;
        }

        public void Update(Particle particle1, Particle particle2)
        {
            Vector3 midl = (particle2.position + particle1.position) * 0.5f;
            Vector3 norm = (particle2.position - particle1.position); norm.Normalize();

            Vector3 goal1 = midl - norm * this.defLen * 0.5f;
            Vector3 goal2 = midl + norm * this.defLen * 0.5f;

            particle1.position = particle1.position + (goal1 - particle1.position) * this.stiffness;

            particle2.position = particle2.position + (goal2 - particle2.position) * this.stiffness;
        }
    }
}
