using System;

namespace MoveOnSphere
{
    public class Entity
    {
        public Vector v = new Vector() { z = 1 };
        public float moveAngle = 0;
        public float rotateAngle = 0;
        public Vector moveAxe = new Vector() { y = 1 };

        public void RandomFill()
        {
            this.rotateAngle = 0.0f;
            this.v.x = Helper.RandomFloat(-1, 1);
            this.v.y = Helper.RandomSign() * Helper.RandomFloat(0, (float)Math.Sqrt(1 - this.v.x * this.v.x));
            this.v.z = Helper.RandomSign() * (float)Math.Sqrt(1 - this.v.x * this.v.x - this.v.y * this.v.y);

            this.moveAngle = 0.0f;
            this.moveAxe = Vector.CrossProductAndNormilize(this.v, World.Vector);
        }

        public void Move()
        {
            Quaternion rotateQuaternion = new Quaternion(this.v, rotateAngle);
            rotateQuaternion.Convert(this.moveAxe);
            Quaternion moveQuaternion = new Quaternion(this.moveAxe, moveAngle);            
            moveQuaternion.Convert(this.v);
        }
    }
}
