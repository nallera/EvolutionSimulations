using System.Collections;
using System.Collections.Generic;

namespace EvolutionSimulations
{
    public partial class CellContent : IEnumerable<int>
    {
        private List<int> creaturesIds;
        private Food foodContent;
        public int CreatureCount { get { return creaturesIds.Count; } }

        public CellContent(CellContent source)
        {
            foodContent = source.foodContent;
            creaturesIds = new List<int>(source.creaturesIds);
        }

        public CellContent()
        {
            foodContent = Food.Empty;
            creaturesIds = new List<int>();
        }

        public void AddFood()
        {
            foodContent = Food.Food;
        }

        public void ClearFood()
        {
            foodContent = Food.Empty;
        }

        public bool HasFood()
        {
            if(foodContent == Food.Food)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<int> GetCreatures()
        {
            return creaturesIds;
        }

        public void AddCreature(int id)
        {
            creaturesIds.Add(id);
        }

        public void ClearCreatures()
        {
            creaturesIds.Clear();
        }

        public IEnumerator<int> GetEnumerator()
        {
            return creaturesIds.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}