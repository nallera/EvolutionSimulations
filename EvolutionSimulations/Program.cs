﻿using EvolutionSimulations.Models.CreatureTypes;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace EvolutionSimulations
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
               //.WriteTo.Console()
               .CreateLogger();

            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();

            SimulationParameters Parameters = new SimulationParameters
            {
                xLimit = 15,
                yLimit = 15,
                simulationDays = 100,
                stepsPerDay = 180,
                foodPerDay = 50,
                foodToSurvive = 1,
                foodToReproduce = 2,
                numberOfSimulations = 100,
                logOnlyPopulation = true
            };

            Terrain simulationTerrain = new Terrain(Parameters.xLimit, Parameters.yLimit);

            CreatureTypeList creatureTypes = new CreatureTypeList();
            creatureTypes.Add(new FriendlyType());
            creatureTypes.Add(new HostileType(), mutationProbability: 0.02);
            creatureTypes.Add(new QuickType(), mutationProbability: 0.02);
            creatureTypes.Add(new BigType(), mutationProbability: 0.02);

            Population initialPopulation = new Population(creatureTypes);

            foreach (ICreatureType creatureType in initialPopulation.CreatureTypes)
            {
                string typesString = "";

                typesString += creatureType.Name + ",";
                Parameters.CreatureTypes.Add(typesString.Trim(','));
            }

            using (StreamWriter file = File.CreateText(Directory.GetCurrentDirectory() + @"\logs\parameters.json"))
            {
                string jsonString = JsonConvert.SerializeObject(Parameters);
                file.Write(jsonString);
            }

            initialPopulation.AddCreature(initialPopulation.CreatureTypes[0]);
            initialPopulation.AddCreature(initialPopulation.CreatureTypes[0]);
            initialPopulation.AddCreature(initialPopulation.CreatureTypes[0]);
            initialPopulation.AddCreature(initialPopulation.CreatureTypes[0]);
            initialPopulation.AddCreature(initialPopulation.CreatureTypes[0]);
            initialPopulation.AddCreature(initialPopulation.CreatureTypes[0]);


            Simulation simulationVar = new Simulation(simulationTerrain, Parameters.simulationDays, Parameters.stepsPerDay,
                initialPopulation, Parameters.foodToSurvive, Parameters.foodToReproduce, Parameters.logOnlyPopulation);
            SimulationResults results = new SimulationResults(Parameters.logOnlyPopulation);

            for (int simulationNumber = 0; simulationNumber < Parameters.numberOfSimulations; simulationNumber++)
            {
                Console.WriteLine($"Simulation #{simulationNumber}");
                simulationVar.SetNewSimulation();
                results.AddSingleSimulationResults(simulationVar.RunSimulation(Parameters.foodPerDay, PositionType.Random));
            }

            results.PrintToFile();

            watch.Stop();

            Log.Information($"Simulation duration: {watch.ElapsedMilliseconds * 0.001} seconds.");
        }
    }
}
