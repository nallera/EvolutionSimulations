﻿using System;
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
        public Population CurrentPopulation;
        public DayStepResult<CreatureList> CreatureResults;
        public DayStepResult<Terrain> TerrainResults;
        public DayStepResult<Population> PopulationResults;

        public int FoodToSurvive;
        public int FoodToReproduce;

        public Simulation(Terrain simulationTerrain, int simulationDays, int stepsPerDay, CreatureList initialCreatures, List<Mutation> mutations, Population populations, int foodToSurvive, int foodToReproduce)
        {
            CurrentTerrain = simulationTerrain;
            CurrentCreatures = new CreatureList(_initialCreatures);
            CurrentPopulation = new Population(populations);
            _simulationDays = simulationDays;
            _stepsPerDay = stepsPerDay;
            _initialCreatures = initialCreatures;
            _mutations = mutations;
            FoodToSurvive = foodToSurvive;
            FoodToReproduce = foodToReproduce;

            CreatureResults = new DayStepResult<CreatureList>();
            TerrainResults = new DayStepResult<Terrain>();
            PopulationResults = new DayStepResult<Population>();
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
                PrintEndOfDayCreatures(CurrentCreatures, day);
                PrintCreaturesNextStatus(CurrentCreatures);
            }

            return new SimulationResults(CreatureResults, TerrainResults, PopulationResults);
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
            CurrentTerrain.RemoveEatenFood();
        }

        private void SetupDayStart(int foodPerDay, PositionType positionType, int day)
        {
            CurrentCreatures.ResetCreatures();
            CurrentCreatures.SetPositions(positionType, CurrentTerrain.X, CurrentTerrain.Y);
            CurrentTerrain.ClearFood();
            CurrentTerrain.AddRandomFood(foodPerDay);
            PrintStartOfDayCreatures(CurrentCreatures, day);
            CurrentPopulation.Update(CurrentCreatures);
        }

        private void PrintStartOfDayCreatures(CreatureList currentCreatures, int day)
        {
            Console.WriteLine($"Creatures that start day {day}:");
            foreach (Creature creature in currentCreatures)
            {
                Console.WriteLine($"Creature ID#{creature.Id}, Health {creature.Health} ({creature.Traits[0]})");
            }
            Console.WriteLine("");
        }

        private void PrintEndOfDayCreatures(CreatureList currentCreatures, int day)
        {
            Console.WriteLine("");
            Console.WriteLine($"Creatures at the end of day {day}:");
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
            StoreStepResults(day);
            DetermineStepActions();
        }

        private void StoreStepResults(int day)
        {
            CreatureResults.AddStep(new CreatureList(CurrentCreatures), day);
            TerrainResults.AddStep(new Terrain(CurrentTerrain), day);
            PopulationResults.AddStep(new Population(CurrentPopulation), day);
        }
    }
}