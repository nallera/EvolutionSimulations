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
                    SimulateStep(day, step, CurrentTerrain.X, CurrentTerrain.Y);
                }

                DetermineCreaturesNextStatus();
                PrintEndOfDayCreatures(CurrentCreatures);
                PrintCreaturesNextStatus(CurrentCreatures);
            }

            return new SimulationResults(CreatureResults, TerrainResults);
        }

        private void PrintCreaturesNextStatus(CreatureList currentCreatures)
        {
            foreach (Creature creature in currentCreatures)
            {
                string phraseAboutHealth = ".";
                string nextStatusReadable = "";

                if(creature.Health <= 0.0)
                {
                    phraseAboutHealth = creature.FoodCollectedLastDay < FoodToSurvive? ", and it was fatally injured in a fight."
                        : $", but unfortunately it was fatally injured in a fight.";
                }

                switch(creature.NextStatus)
                {
                    case LifeStatus.Die:
                        nextStatusReadable = "die";
                        break;
                    case LifeStatus.Reproduce:
                        nextStatusReadable = "reproduce and have offspring";
                        break;
                    case LifeStatus.StayAlive:
                        nextStatusReadable = "stay alive";
                        break;
                }

                Console.WriteLine($"Creature ID#{creature.Id} collected {creature.FoodCollectedLastDay} food{phraseAboutHealth} So, next day, it will {nextStatusReadable}.");
            }
            Console.WriteLine("");
        }

        //private void PrintTerrainStep(string message)
        //{
        //    Console.WriteLine(message);
        //    CurrentTerrain.PrintTerrain();
        //    Console.WriteLine("");
        //}

        private void DetermineCreaturesNextStatus()
        {
            CurrentCreatures.DetermineCreaturesNextStatus(FoodToSurvive, FoodToReproduce);
        }

        private void DetermineStepActions()
        {
            CurrentCreatures.CheckSurroundings(CurrentTerrain.FoodUnits);
            CurrentCreatures.CheckInteractions(CurrentTerrain.FoodUnits);
        }

        private void SetupDayStart(int foodPerDay, PositionType positionType, int day)
        {
            CurrentCreatures.ResetCreatures();
            CurrentCreatures.SetPositions(positionType, CurrentTerrain.X, CurrentTerrain.Y);
            CurrentTerrain.ClearFood();
            CurrentTerrain.AddRandomFood(foodPerDay);
            PrintStartOfDayCreatures(CurrentCreatures);
        }

        private void PrintStartOfDayCreatures(CreatureList currentCreatures)
        {
            Console.WriteLine("Creatures that start this day:");
            foreach (Creature creature in currentCreatures)
            {
                Console.WriteLine($"Creature ID#{creature.Id}, Health {creature.Health} ({creature.Traits[0]})");
            }
            Console.WriteLine("");
        }

        private void PrintEndOfDayCreatures(CreatureList currentCreatures)
        {
            Console.WriteLine("Creatures at the end of this day:");
            foreach (Creature creature in currentCreatures)
            {
                Console.WriteLine($"Creature ID#{creature.Id}, Health {creature.Health} ({creature.Traits[0]})");
            }
            Console.WriteLine("");
        }

        private void SimulateStep(int day, int step, double xLimit, double yLimit)
        {
            foreach (Creature creature in CurrentCreatures)
            {
                Console.WriteLine($"Creature ID#{creature.Id} position X:{creature.Position.X:N2}, Y:{creature.Position.Y:N2}");
            }
            CurrentCreatures.MoveCreatures(xLimit, yLimit);
            StoreStepResults(day, step);
            DetermineStepActions();
        }

        private void StoreStepResults(int day, int step)
        {
            CreatureResults.AddStep(new CreatureList(CurrentCreatures), day, step);
            TerrainResults.AddStep(new Terrain(CurrentTerrain), day, step);
        }
    }
}