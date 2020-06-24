using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EvolutionSimulations
{
    public class Population: IEnumerable<CreatureType>
    {
        public List<CreatureType> CreatureTypes { get; set; }

        public Population()
        {
            CreatureTypes = new List<CreatureType>();
        }
        public Population(List<CreatureType> creatureTypes)
        {
            CreatureTypes = new List<CreatureType>(creatureTypes);
        }

        public Population(Population source)
        {
            CreatureTypes = new List<CreatureType>(source.CreatureTypes);
        }

        public CreatureType this[int index]
        {
            get
            {
                return CreatureTypes[index];
            }
            set
            {
                CreatureTypes[index] = value;
            }
        }

        public void creatures_CreatureAdded(object sender, CreatureTraitsEventArgs e)
        {
            int index = CreatureTypes.FindIndex(creatureType => creatureType.Traits.SequenceEqual(e.Traits));
            CreatureTypes[index].NumberOfCreatures++;
        }

        public void creatures_CreatureRemoved(object sender, CreatureTraitsEventArgs e)
        {
            int index = CreatureTypes.FindIndex(creatureType => creatureType.Traits.SequenceEqual(e.Traits));
            CreatureTypes[index].NumberOfCreatures--;
        }

        public IEnumerator<CreatureType> GetEnumerator()
        {
            return CreatureTypes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}