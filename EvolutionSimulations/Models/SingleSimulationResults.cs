using EvolutionSimulations.DTOs;
using System.Collections.Generic;

namespace EvolutionSimulations
{
    public class SingleSimulationResults
    {
        public DayStepResult<CreatureListDTO> CreatureResults;
        public DayStepResult<Terrain> TerrainResults;
        public DayStepResult<List<int>> PopulationResults;
        public bool _logOnlyPopulation;

        public SingleSimulationResults(DayStepResult<CreatureListDTO> creatureResults, DayStepResult<Terrain> terrainResults,
            DayStepResult<List<int>> populationResults, bool logOnlyPopulation)
        {
            CreatureResults = creatureResults;
            TerrainResults = terrainResults;
            PopulationResults = populationResults;
            _logOnlyPopulation = logOnlyPopulation;
        }
    }
}