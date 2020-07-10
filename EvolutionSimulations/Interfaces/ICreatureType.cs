using System;
using System.Collections.Generic;
using System.Text;

namespace EvolutionSimulations.Interfaces
{
    interface ICreatureType
    {
        public string Name { get; }
        public int NumberOfCreatures { get; }

        public void AddCreature();
        public void RemoveCreature();
    }
}
