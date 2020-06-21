﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;

namespace EvolutionSimulations
{
    internal class SimulationResults
    {
        public DayStepResult<CreatureList> CreatureSteps;
        public DayStepResult<Terrain> TerrainSteps;
        public DayStepResult<Population> PopulationSteps;

        public SimulationResults(DayStepResult<CreatureList> creatureResults, DayStepResult<Terrain> terrainResults, DayStepResult<Population> populationResults)
        {
            CreatureSteps = creatureResults;
            TerrainSteps = terrainResults;
            PopulationSteps = populationResults;
        }

        internal void PrintToFile(string fileName)
        {
            if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\logs")) Directory.CreateDirectory((Directory.GetCurrentDirectory() + @"\logs"));
            if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\logs\" + fileName)) Directory.CreateDirectory((Directory.GetCurrentDirectory() + @"\logs\" + fileName));
            using (StreamWriter file = File.CreateText(Directory.GetCurrentDirectory() + @"\logs\" + fileName + @"\creatures.json"))
            {
                string jsonString = JsonConvert.SerializeObject(CreatureSteps, Formatting.Indented);
                file.Write(jsonString);
            }

            using (StreamWriter file = File.CreateText(Directory.GetCurrentDirectory() + @"\logs\" + fileName + @"\terrain.json"))
            {
                string jsonString = JsonConvert.SerializeObject(CreatureSteps, Formatting.Indented);
                file.Write(jsonString);
            }

            using (StreamWriter file = File.CreateText(Directory.GetCurrentDirectory() + @"\logs\" + fileName + @"\population.json"))
            {
                string jsonString = JsonConvert.SerializeObject(PopulationSteps, Formatting.Indented);
                file.Write(jsonString);
            }
        }
    }
}