using System;
using System.Collections.Generic;
using System.Text;

namespace EvolutionSimulations.DTOs
{
    public class CreatureListDTO
    {
        public List<CreatureDTO> creatures;

        public CreatureListDTO()
        {
            creatures = new List<CreatureDTO>();
        }

        public CreatureListDTO(CreatureList source)
        {
            creatures = new List<CreatureDTO>();

            foreach (Creature creature in source)
            {
                creatures.Add(new CreatureDTO(creature));
            }
        }
    }
}
