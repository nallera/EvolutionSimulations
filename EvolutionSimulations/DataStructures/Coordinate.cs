using System;

namespace EvolutionSimulations
{
    public class Coordinate
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Coordinate(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Coordinate(Coordinate source)
        {
            X = source.X;
            Y = source.Y;
        }

        public static Coordinate operator +(Coordinate a) => a; 
        public static Coordinate operator +(Coordinate a, Coordinate b)
         => new Coordinate(a.X + b.X , a.Y + b.Y);
        public static Coordinate operator -(Coordinate a) => new Coordinate(-a.X, -a.Y);
        public static Coordinate operator -(Coordinate a, Coordinate b) => a + (-b);

        internal bool InReach(Coordinate position, double reach)
        {
            return Math.Sqrt(Math.Pow((this - position).X,2) + Math.Pow((this - position).Y,2)) < reach ? true : false;
        }
    }
}