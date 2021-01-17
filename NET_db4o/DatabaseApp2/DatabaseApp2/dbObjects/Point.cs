using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseApp2.dbObjects
{
    class Point
    {
        public float x;
        public float y;

        public Point(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}", this.x, this.y);
        }

        public void Move(float addX, float addY)
        {
            this.x += addX;
            this.y += addY;
        }
    }
}
