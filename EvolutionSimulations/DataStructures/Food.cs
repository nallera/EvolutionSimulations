using System.Collections.Generic;

namespace EvolutionSimulations
{
    public class Food
    {
        public Coordinate Position { get; set; }
        public List<int> ReachingCreatures;
        public bool Eaten;

        public Food()
        {
            Position = new Coordinate(0.0, 0.0);
            ReachingCreatures = new List<int>();
            Eaten = false;
        }

        public Food(Food source)
        {
            Position = new Coordinate(source.Position);
            ReachingCreatures = new List<int>(source.ReachingCreatures);
            Eaten = source.Eaten;
        }

        public Food(double x, double y)
        {
            Position = new Coordinate(x, y);
            ReachingCreatures = new List<int>();
            Eaten = false;
        }
    }
}