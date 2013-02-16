using System.Drawing;

namespace Entities
{
    interface IEntity
    {
        float X { get; set; }
        float Y { get; set; }
        void setPosition(float pX, float pY);

        float CenterX { get; set; }
        float CenterY { get; set; }
        void setCenterPosition(float pCenterX, float pCenterY);

        float BaseWidth { get; set; }
        float BaseHeight { get; set; }

        float Width { get; set; }
        float Height { get; set; }

        float Angle { get; set; }
        float VectorX { get; }
        float VectorY { get; }
        float Speed { get; set; }


        float Health { get; set; }

        void onManagedDraw(Graphics graphics);

        void onManagedUpdate(float pSecondsElapsed);
    }
}