using System.Collections.Generic;

namespace EvolutionSimulations
{
    public class CreatureIdAndPositionandReachingCreatures
    {
        public int CreatureId;
        public Coordinate Position;
        public List<int> ReachingCreaturesIds;

        public CreatureIdAndPositionandReachingCreatures()
        {
            CreatureId = -1;
            Position = new Coordinate(-1.0, -1.0);
            ReachingCreaturesIds = new List<int>();
        }
    }
}