using System.Collections.Generic;

namespace EvolutionSimulations
{
    internal class SimulationResults
    {
        public DayStepResult<CreatureList> CreatureSteps;
        public DayStepResult<Terrain> TerrainSteps;

        public SimulationResults(DayStepResult<CreatureList> creatureResults, DayStepResult<Terrain> terrainResults)
        {
            CreatureSteps = creatureResults;
            TerrainSteps = terrainResults;
        }
    }
}