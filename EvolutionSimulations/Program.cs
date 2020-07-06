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

            SimulationParameters Parameters = new SimulationParameters
            {
                xLimit = 15,
                yLimit = 15,
                simulationDays = 50,
                stepsPerDay = 20,
                foodPerDay = 80,
                foodToSurvive = 1,
                foodToReproduce = 2,
                numberOfSimulations = 500,
                logOnlyPopulation = true
            };

            Terrain simulationTerrain = new Terrain(Parameters.xLimit, Parameters.yLimit);

            Population initialPopulation = new Population(new List<CreatureType>
            {
                new CreatureType { CreatureTrait.Friendly },
                new CreatureType { CreatureTrait.Hostile }
            }, new CreatureCharacteristics
            {
                Health = 100.0,
                AttackPower = 10.0,
                MaxSpeed = 1.0,
                Reach = 1.0,
                Energy = (double)Parameters.stepsPerDay * 2
            }) ;

            foreach (CreatureType creatureType in initialPopulation.CreatureTypes)
            {
                string traitsString = "";

                foreach (CreatureTrait trait in creatureType.Traits)
                {
                    traitsString += trait.ToString() + ",";
                }

                Parameters.CreatureTypes.Add(traitsString.Trim(','));
            }

            using (StreamWriter file = File.CreateText(Directory.GetCurrentDirectory() + @"\logs\parameters.json"))
            {
                string jsonString = JsonConvert.SerializeObject(Parameters);
                file.Write(jsonString);
            }

            initialPopulation.AddNewCreature(initialPopulation.CreatureTypes[0]);
            initialPopulation.AddNewCreature(initialPopulation.CreatureTypes[1]);
            initialPopulation.AddNewCreature(initialPopulation.CreatureTypes[0]);
            initialPopulation.AddNewCreature(initialPopulation.CreatureTypes[0]);
            initialPopulation.AddNewCreature(initialPopulation.CreatureTypes[1]);
            initialPopulation.AddNewCreature(initialPopulation.CreatureTypes[1]);


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
        }
    }
}
