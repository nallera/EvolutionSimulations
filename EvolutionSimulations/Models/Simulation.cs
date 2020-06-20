using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

namespace EvolutionSimulations
{
    internal class Simulation
    {
        private int _simulationDays;
        private int _stepsPerDay;
        private CreatureList _initialCreatures;
        private List<Mutation> _mutations;

        public Terrain CurrentTerrain;
        public CreatureList CurrentCreatures;
        DayStepResult<CreatureList> CreatureResults;
        DayStepResult<Terrain> TerrainResults;

        public int FoodToSurvive;
        public int FoodToReproduce;

        public Simulation(Terrain simulationTerrain, int simulationDays, int stepsPerDay, CreatureList initialCreatures, List<Mutation> mutations, int foodToSurvive, int foodToReproduce)
        {
            CurrentTerrain = simulationTerrain;
            _simulationDays = simulationDays;
            _stepsPerDay = stepsPerDay;
            _initialCreatures = initialCreatures;
            CurrentCreatures = new CreatureList(_initialCreatures);
            _mutations = mutations;
            FoodToSurvive = foodToSurvive;
            FoodToReproduce = foodToReproduce;

            CreatureResults = new DayStepResult<CreatureList>(_simulationDays, _stepsPerDay);
            TerrainResults = new DayStepResult<Terrain>(_simulationDays, _stepsPerDay);
        }

        public SimulationResults RunSimulation(int foodPerDay, PositionType positionType)
        {
            for (int day = 0; day < _simulationDays; day++)
            {
                CurrentCreatures.UpdateCreatures();
                if (CurrentCreatures.Count == 0)
                {
                    Console.WriteLine($"All the creatures died on day {day - 1}, so no more days will be simulated.");
                    break;
                }
                SetupDayStart(foodPerDay, positionType, day);

                for (int step = 0; step < _stepsPerDay; step++)
                {
                    SimulateStep(day, step);
                }

                DetermineCreaturesNextStatus();
                PrintCreaturesNextStatus(CurrentCreatures);
            }

            return new SimulationResults(CreatureResults, TerrainResults);
        }

        private void PrintCreaturesNextStatus(CreatureList currentCreatures)
        {
            foreach (Creature creature in currentCreatures)
            {
                Console.WriteLine($"Creature ID#{creature.Id} collected {creature.FoodCollectedLastDay} food. So, next day, it will {creature.NextStatus}.");
            }
            Console.WriteLine("");
        }

        private void PrintTerrainStep(string message)
        {
            Console.WriteLine(message);
            CurrentTerrain.PrintTerrain();
            Console.WriteLine("");
        }

        private void DetermineCreaturesNextStatus()
        {
            CurrentCreatures.DetermineCreaturesNextStatus(FoodToSurvive, FoodToReproduce);
        }

        private void DetermineStepActions()
        {
            CurrentCreatures.ProcessInteractions(CurrentTerrain.CellsOutcome());
        }

        private void SetupDayStart(int foodPerDay, PositionType positionType, int day)
        {
            CurrentCreatures.Reset();
            CurrentTerrain.ClearCreaturePositions();
            CurrentCreatures.SetPositions(positionType);
            CurrentTerrain.UpdateCreaturePositions(CurrentCreatures.GetPositions());
            CurrentTerrain.ClearFood();
            CurrentTerrain.AddRandomFood(foodPerDay);
            PrintTerrainStep($"Start of day {day}:");
            PrintAliveCreatures(CurrentCreatures);
        }

        private void PrintAliveCreatures(CreatureList currentCreatures)
        {
            Console.WriteLine("Creatures that start this day:");
            foreach (Creature creature in currentCreatures)
            {
                Console.WriteLine($"Creature ID#{creature.Id}");
            }
            Console.WriteLine("");
        }

        private void SimulateStep(int day, int step)
        {
            CurrentCreatures.Move();
            CurrentTerrain.UpdateCreaturePositions(CurrentCreatures.GetPositions());
            StoreStepResults(day, step);
            PrintTerrainStep($"Day {day}, step {step}:");
            DetermineStepActions();
        }

        private void StoreStepResults(int day, int step)
        {
            CreatureResults.AddStep(new CreatureList(CurrentCreatures), day, step);
            TerrainResults.AddStep(new Terrain(CurrentTerrain), day, step);
        }
    }
}