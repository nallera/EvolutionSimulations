using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Data;

namespace EvolutionSimulations
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
               //.WriteTo.Console()
               .CreateLogger();

            int xLimit = 15;
            int yLimit = 15;

            Terrain simulationTerrain = new Terrain(xLimit, yLimit);

            int simulationDays = 50;
            int stepsPerDay = 20;
            int foodPerDay = 50;
            int foodToSurvive = 1;
            int foodToReproduce = 2;
            int numberOfSimulations = 100;
            bool logOnlyPopulation = true;

            Population initialPopulation = new Population(new List<CreatureType>
            {
                new CreatureType { CreatureTrait.Friendly },
                new CreatureType { CreatureTrait.Hostile }
            });

            initialPopulation.AddNewCreature(initialPopulation.CreatureTypes[0]);
            initialPopulation.AddNewCreature(initialPopulation.CreatureTypes[1]);
            initialPopulation.AddNewCreature(initialPopulation.CreatureTypes[0]);
            initialPopulation.AddNewCreature(initialPopulation.CreatureTypes[0]);
            initialPopulation.AddNewCreature(initialPopulation.CreatureTypes[1]);
            initialPopulation.AddNewCreature(initialPopulation.CreatureTypes[1]);

            Simulation simulationVar = new Simulation(simulationTerrain, simulationDays, stepsPerDay,
                initialPopulation, foodToSurvive, foodToReproduce, logOnlyPopulation);
            SimulationResults results = new SimulationResults(logOnlyPopulation);

            for (int simulationNumber = 0; simulationNumber < numberOfSimulations; simulationNumber++)
            {
                simulationVar.SetNewSimulation();
                results.AddSingleSimulationResults(simulationVar.RunSimulation(foodPerDay, PositionType.Random));
            }

            results.PrintToFile();
        }
    }
}
