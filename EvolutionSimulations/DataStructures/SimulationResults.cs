using EvolutionSimulations.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;

namespace EvolutionSimulations
{
    internal class SimulationResults
    {
        public DayStepResult<CreatureListDTO> CreatureSteps;
        public DayStepResult<Terrain> TerrainSteps;
        public DayStepResult<List<int>> PopulationSteps;

        public SimulationResults(DayStepResult<CreatureListDTO> creatureResults, DayStepResult<Terrain> terrainResults, DayStepResult<List<int>> populationResults)
        {
            CreatureSteps = creatureResults;
            TerrainSteps = terrainResults;
            PopulationSteps = populationResults;
        }

        internal void PrintToFile()
        {
            if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\logs")) Directory.CreateDirectory((Directory.GetCurrentDirectory() + @"\logs"));
            using (StreamWriter file = File.CreateText(Directory.GetCurrentDirectory() + @"\logs\creatures.json"))
            {
                string jsonString = JsonConvert.SerializeObject(CreatureSteps);
                file.Write(jsonString);
            }

            using (StreamWriter file = File.CreateText(Directory.GetCurrentDirectory() + @"\logs\terrain.json"))
            {
                string jsonString = JsonConvert.SerializeObject(TerrainSteps);
                file.Write(jsonString);
            }

            using (StreamWriter file = File.CreateText(Directory.GetCurrentDirectory() + @"\logs\population.json"))
            {
                string jsonString = JsonConvert.SerializeObject(PopulationSteps);
                file.Write(jsonString);
            }
        }
    }
}