using System;

namespace MoveOnSphere
{
    public class Entity
    {
        public Vector v = new Vector() { z = 1 };
        public Vector v_ = new Vector() { x = 1 };
        public float moveAngle = 0;
        public float rotateAngle = 0;

		public void Recalculate ()
		{
			// TODO: Check.
			Vector vt = new Vector();
			vt.FillAsVectorProduction(this.v, this.v_);
			this.v_.FillAsVectorProduction(vt, this.v);
		}

        public void Move()
        {
			TransformationAsQuaternion qRotate = new TransformationAsQuaternion();
			qRotate.Fill(this.v, rotateAngle);
            qRotate.Transform(this.v_);
			TransformationAsQuaternion qMove = new TransformationAsQuaternion();
			qMove.Fill(this.v_, moveAngle);            
            qMove.Transform(this.v);
        }

        public void RandomFill()
        {
            this.rotateAngle = 0.0f;
            this.v.x = Helper.RandomFloat(-1, 1);
            this.v.y = Helper.RandomSign() * Helper.RandomFloat(0, (float)Math.Sqrt(1 - this.v.x * this.v.x));
            this.v.z = Helper.RandomSign() * (float)Math.Sqrt(1 - this.v.x * this.v.x - this.v.y * this.v.y);

            this.moveAngle = 0.0f;
            this.v_.FillAsVectorProduction(this.v, World.VectorZ);
        }
	}
}
