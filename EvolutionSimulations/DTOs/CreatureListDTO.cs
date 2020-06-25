using System;
using System.Collections.Generic;
using System.Text;

namespace EvolutionSimulations.DTOs
{
    class CreatureListDTO
    {
        private List<Creature> creatures;

        public CreatureListDTO()
        {
            creatures = new List<Creature>();
        }

        public CreatureListDTO(CreatureList source)
        {
            foreach (Creature creature in source)
            {
                creatures.Add(new CreatureDTO(creature));
            }
        }
    }
}
