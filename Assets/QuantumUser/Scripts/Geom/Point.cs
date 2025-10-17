using System;

namespace AGS.Geom
{
    [Serializable]
    public struct Point
    {
        public int x, y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return string.Format("x:{0}, y:{1}", x, y);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Point))
                return false;

            var pt = (Point)obj;
            return pt.x == x && pt.y == y;
        }

        public int Angle()
        {
            return (int)(Math.Atan2(y, x) * 180.0f / Math.PI);
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() << 2;
        }

        public static Point operator + (Point p1, Point p2)
        {
            return new Point(p1.x + p2.x, p1.y + p2.y);
        }

        public static Point operator - (Point p1, Point p2)
        {
            return new Point(p1.x - p2.x, p1.y - p2.y);
        }

		public static Point operator - (Point p)
		{
			return new Point(-p.x, -p.y);
		}
			
		public static bool operator == (Point p1, Point p2)
        {
            return p1.Equals(p2);
        }

        public static bool operator != (Point p1, Point p2)
        {
            return !(p1 == p2);
        }

        public static Point Zero
        {
            get { return new Point(0, 0); }
        }

        public static Point Rotate(Point p, int a)
        {
            var angle = a * Math.PI /180.0f;
            var cos = Math.Cos(angle);
            var sin = Math.Sin(angle);

            var x = p.x * cos - p.y * sin;
            var y = p.x * sin + p.y * cos;

            return new Point((int)Math.Round(x), (int)Math.Round(y));
        }
    }
}
