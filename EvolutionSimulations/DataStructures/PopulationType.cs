using System.Collections;
using System.Collections.Generic;

namespace EvolutionSimulations
{
    public class PopulationType : IEnumerable<CreatureTrait>
    {
        public List<CreatureTrait> Traits { get; set; }

        public PopulationType()
        {
            Traits = new List<CreatureTrait>();
        }

        public PopulationType(PopulationType source)
        {
            Traits = new List<CreatureTrait>(source.Traits);
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