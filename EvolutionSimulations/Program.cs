using System;
using System.Collections.Generic;
using System.Data;

namespace EvolutionSimulations
{
    class Program
    {
        static void Main(string[] args)
        {
            int xLimit = 5;
            int yLimit = 15;

            Terrain simulationTerrain = new Terrain(xLimit, yLimit);

            int simulationDays = 5;
            int stepsPerDay = 4;
            int foodPerDay = 20;
            int foodToSurvive = 1;
            int foodToReproduce = 2;
            CreatureList initialCreatures = new CreatureList();
            List<Mutation> mutations = new List<Mutation>();

            initialCreatures.AddNewCreature(xLimit, yLimit, new List<CreatureTreat> { CreatureTreat.Friendly });
            initialCreatures.AddNewCreature(xLimit, yLimit, new List<CreatureTreat> { CreatureTreat.Friendly });
            initialCreatures.AddNewCreature(xLimit, yLimit, new List<CreatureTreat> { CreatureTreat.Friendly });
            initialCreatures.AddNewCreature(xLimit, yLimit, new List<CreatureTreat> { CreatureTreat.Friendly });

            Simulation simulationVar = new Simulation(simulationTerrain, simulationDays, stepsPerDay, initialCreatures, mutations, foodToSurvive, foodToReproduce);

            SimulationResults results = simulationVar.RunSimulation(foodPerDay, PositionType.Random);

            //for (int day = 0; day < simulationDays; day++)
            //{
            //    for(int step = 0; step < stepsPerDay; step++)
            //    {
            //        Console.WriteLine($"Day {day}, step {step}:");

            //        simulationTerrain.UpdateCreaturePositions(results.CreatureSteps.GetDayStep(day,step).GetPositions());
            //        simulationTerrain.PrintTerrain();
            //    }
            //}
        }
    }
}
