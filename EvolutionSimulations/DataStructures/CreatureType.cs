using System.Collections;
using System.Collections.Generic;

namespace EvolutionSimulations
{
    public class CreatureType : IEnumerable<CreatureTrait>
    {
        public List<CreatureTrait> Traits { get; set; }
        public int NumberOfCreatures { get; set; }

        public CreatureType()
        {
            Traits = new List<CreatureTrait>();
            NumberOfCreatures = 0;
        }

        public CreatureType(CreatureType source)
        {
            Traits = new List<CreatureTrait>(source.Traits);
            NumberOfCreatures = source.NumberOfCreatures;
        }

        public void Add(CreatureTrait trait)
        {
            Traits.Add(trait);
        }

        public IEnumerator<CreatureTrait> GetEnumerator()
        {
            return Traits.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}