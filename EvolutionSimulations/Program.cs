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
            int yLimit = 5;

            Terrain simulationTerrain = new Terrain(xLimit, yLimit);

            int simulationDays = 5;
            int stepsPerDay = 4;
            int foodPerDay = 3;
            int foodToSurvive = 1;
            int foodToReproduce = 2;
            CreatureList initialCreatures = new CreatureList();
            List<Mutation> mutations = new List<Mutation>();

            initialCreatures.AddNewCreature(new List<CreatureTrait> { CreatureTrait.Friendly });
            initialCreatures.AddNewCreature(new List<CreatureTrait> { CreatureTrait.Hostile });
            initialCreatures.AddNewCreature(new List<CreatureTrait> { CreatureTrait.Friendly });
            initialCreatures.AddNewCreature(new List<CreatureTrait> { CreatureTrait.Friendly });

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
