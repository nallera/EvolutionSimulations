using System.Collections.Generic;
using System.Threading;

namespace EvolutionSimulations
{
    public class CreatureInteraction
    {
        private readonly List<int> creaturesIds;
        private readonly bool foodInvolved;
        public int CreaturesCount { get { return creaturesIds.Count; } }

        public CreatureInteraction(List<int> ids, bool hasFood)
        {
            creaturesIds = new List<int>(ids);
            foodInvolved = hasFood;
        }

        public List<int> GetIds()
        {
            return creaturesIds;
        }

        public bool FoodInvolved()
        {
            return foodInvolved;
        }
    }
}