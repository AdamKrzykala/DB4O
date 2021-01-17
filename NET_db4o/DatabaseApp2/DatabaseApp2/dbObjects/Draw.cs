using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DatabaseApp2.dbObjects
{
    class Draw
    {
        public string drawName;
        public Collection<Point> points;

        public Draw(string name,  Collection<Point> tempCollection)
        {
            this.points = tempCollection;
            this.drawName = name;
        }

        public void addPoint(int x, int y)
        {
            this.points.Add(new Point(x, y));
        }

        public void MovePoints(float x, float y)
        {
            foreach (Point point in this.points)
            {
                point.Move(x, y);
            }
        }

        public override string ToString()
        {
            string pointsString = string.Format("{0}:  ", this.drawName);
            foreach (object item in points)
            {
                pointsString += string.Format(" [{0}] ", item.ToString());
            }

            return pointsString;
        }
    }
}
