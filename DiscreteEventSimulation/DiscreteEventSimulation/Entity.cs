using System;

namespace DiscreteEventSimulation
{
    public class Entity
    {
        public int Id;

        public float X, Y; // Coordinates.
        public float V, A; // Speed and rotate angle.
        public float VX, VY; // Moving vector.
        public void SetV(float v, float a)
        {
            this.V = v;
            this.A = a;
            this.VX = v * (float)System.Math.Cos(a);
            this.VY = v * (float)System.Math.Sin(a);
        }

        public float R; // Radius.

        public int i, j, indexInCell; // Cell indexes.

        public float Time { get; private set; } // Time.
        public int Event { get; private set; } // Next event type.
        public Entity Next { get; private set; } // Next collision event entity.


        public void ClearEvent()
        {
            this.Event = 0;
            this.Time = float.PositiveInfinity;
            this.Next = null;
        }

        public void SetEvent(float time, int @event, Entity next = null)
        {
            if (this.Time > time)
            {
                this.Event = @event;
                this.Time = time;
                this.Next = next;
            }
        }

        public void Update(float time)
        {
            this.X += time * this.VX;
            this.Y += time * this.VY;
            this.Time -= time;
        }
    }
}
