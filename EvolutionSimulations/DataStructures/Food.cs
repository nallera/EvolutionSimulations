using System.Collections.Generic;

namespace EvolutionSimulations
{
    public class Food
    {
        public Coordinate Position { get; set; }
        public List<int> ReachingCreatures;

        public Food()
        {
            Position = new Coordinate(0.0, 0.0);
            ReachingCreatures = new List<int>();
        }

        public Food(Food source)
        {
            Position = new Coordinate(source.Position);
            ReachingCreatures = new List<int>(source.ReachingCreatures);
        }

        public Food(double x, double y)
        {
            Position = new Coordinate(x, y);
            ReachingCreatures = new List<int>();
        }
    }
}