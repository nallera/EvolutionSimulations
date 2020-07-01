using EvolutionSimulations.DTOs;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;

namespace EvolutionSimulations
{
    internal class SimulationResults
    {
        public List<DayStepResult<CreatureListDTO>> CreatureSteps;
        public List<DayStepResult<Terrain>> TerrainSteps;
        public List<DayStepResult<List<int>>> PopulationSteps;

        private readonly bool _logOnlyPopulation;

        public SimulationResults(bool logOnlyPopulation)
        {
            _logOnlyPopulation = logOnlyPopulation;

            if (!_logOnlyPopulation)
            {
                CreatureSteps = new List<DayStepResult<CreatureListDTO>>();
                TerrainSteps = new List<DayStepResult<Terrain>>();
            }

            PopulationSteps = new List<DayStepResult<List<int>>>();
        }

        public SimulationResults(List<DayStepResult<CreatureListDTO>> creatureResults, List<DayStepResult<Terrain>> terrainResults,
            List<DayStepResult<List<int>>> populationResults, bool logOnlyPopulation)
        {
            _logOnlyPopulation = logOnlyPopulation;

            if (!_logOnlyPopulation)
            {
                CreatureSteps = creatureResults;
                TerrainSteps = terrainResults;
            }
            
            PopulationSteps = populationResults;
        }

        internal void PrintToFile()
        {
            if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\logs")) Directory.CreateDirectory((Directory.GetCurrentDirectory() + @"\logs"));

            if (!_logOnlyPopulation)
            {
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
            }

            using (StreamWriter file = File.CreateText(Directory.GetCurrentDirectory() + @"\logs\population.json"))
            {
                string jsonString = JsonConvert.SerializeObject(PopulationSteps);
                file.Write(jsonString);
            }
        }

        internal void AddSingleSimulationResults(SingleSimulationResults singleSimulationResults)
        {
            if (!_logOnlyPopulation)
            {
                CreatureSteps.Add(singleSimulationResults.CreatureResults);
                TerrainSteps.Add(singleSimulationResults.TerrainResults);
            }
            PopulationSteps.Add(singleSimulationResults.PopulationResults);
        }
    }
}