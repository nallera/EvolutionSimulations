using System;
using System.Collections.Generic;

namespace EvolutionSimulations
{
    public class Population
    {
        public List<PopulationType> PopulationTypes { get; set; }

        public Population(List<PopulationType> populationTypes)
        {
            PopulationTypes = new List<PopulationType>(populationTypes);
        }

        public Population(Population source)
        {
            PopulationTypes = new List<PopulationType>(source.PopulationTypes);
        }

        internal void Update(CreatureList currentCreatures)
        {
            foreach (Creature creature in currentCreatures)
            {
                //PopulationTypes.FindIndex(pop => pop.Traits.Contains(creature.Traits));
            }
        }
    }
}