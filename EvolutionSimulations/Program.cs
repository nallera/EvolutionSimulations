using System;
using System.Collections.Generic;
using System.Data;

namespace EvolutionSimulations
{
    class Program
    {
        static void Main(string[] args)
        {
            int xLimit = 15;
            int yLimit = 15;

            Terrain simulationTerrain = new Terrain(xLimit, yLimit);

            int simulationDays = 5;
            int stepsPerDay = 4;
            int foodPerDay = 100;
            int foodToSurvive = 1;
            int foodToReproduce = 2;
            CreatureList initialCreatures = new CreatureList();
            List<Mutation> mutations = new List<Mutation>();
            Population populations = new Population(new List<PopulationType>
            {
                new PopulationType { CreatureTrait.Friendly }, 
                new PopulationType { CreatureTrait.Hostile },
                new PopulationType { CreatureTrait.Friendly, CreatureTrait.Hostile },
            });

            initialCreatures.AddNewCreature(populations.PopulationTypes[0]);
            initialCreatures.AddNewCreature(populations.PopulationTypes[1]);
            initialCreatures.AddNewCreature(populations.PopulationTypes[0]);
            initialCreatures.AddNewCreature(populations.PopulationTypes[0]);
            initialCreatures.AddNewCreature(populations.PopulationTypes[1]);
            initialCreatures.AddNewCreature(populations.PopulationTypes[1]);

            Simulation simulationVar = new Simulation(simulationTerrain, simulationDays, stepsPerDay, initialCreatures, mutations, populations, foodToSurvive, foodToReproduce);

            SimulationResults results = simulationVar.RunSimulation(foodPerDay, PositionType.Random);

            results.PrintToFile($"run_{DateTime.Now.ToString("yyyyMMdd.HHmmss")}");
        }
    }
}
