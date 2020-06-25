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

            int simulationDays = 10;
            int stepsPerDay = 10;
            int foodPerDay = 70;
            int foodToSurvive = 1;
            int foodToReproduce = 2;
            CreatureList creatures = new CreatureList();
            List<Mutation> mutations = new List<Mutation>();
            Population populations = new Population(new List<CreatureType>
            {
                new CreatureType { CreatureTrait.Friendly }, 
                new CreatureType { CreatureTrait.Hostile },
                new CreatureType { CreatureTrait.Friendly, CreatureTrait.Hostile },
            });

            creatures.CreatureAdded += populations.creatures_CreatureAdded;
            creatures.CreatureRemoved += populations.creatures_CreatureRemoved;

            creatures.AddNewCreature(populations[0]);
            creatures.AddNewCreature(populations[1]);
            creatures.AddNewCreature(populations[0]);
            creatures.AddNewCreature(populations[0]);
            creatures.AddNewCreature(populations[1]);
            creatures.AddNewCreature(populations[1]);

            Simulation simulationVar = new Simulation(simulationTerrain, simulationDays, stepsPerDay, creatures, mutations, populations, foodToSurvive, foodToReproduce);

            SimulationResults results = simulationVar.RunSimulation(foodPerDay, PositionType.Random);

            results.PrintToFile($"run_{DateTime.Now.ToString("yyyyMMdd.HHmmss")}");
        }
    }
}
