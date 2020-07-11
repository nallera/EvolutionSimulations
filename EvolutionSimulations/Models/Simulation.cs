using EvolutionSimulations.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using Serilog;

namespace EvolutionSimulations
{
    internal class Simulation
    {
        private int _simulationDays;
        private int _stepsPerDay;

        private readonly Population _initialPopulation;

        public Terrain CurrentTerrain;
        public Population CurrentPopulation;
        public DayStepResult<CreatureListDTO> CreatureResults;
        public DayStepResult<Terrain> TerrainResults;
        public DayStepResult<List<int>> PopulationResults;

        public int FoodToSurvive;
        public int FoodToReproduce;

        private readonly bool _logOnlyPopulation;

        public Simulation(Terrain simulationTerrain, int simulationDays, int stepsPerDay,
            Population initialPopulation, int foodToSurvive, int foodToReproduce, bool logOnlyPopulation)
        {
            CurrentTerrain = simulationTerrain;
            
            _simulationDays = simulationDays;
            _stepsPerDay = stepsPerDay;
            FoodToSurvive = foodToSurvive;
            FoodToReproduce = foodToReproduce;

            _logOnlyPopulation = logOnlyPopulation;

            _initialPopulation = initialPopulation;
            CurrentPopulation = new Population(_initialPopulation);

            if(!_logOnlyPopulation)
            {
                CreatureResults = new DayStepResult<CreatureListDTO>();
                TerrainResults = new DayStepResult<Terrain>();
            }

            PopulationResults = new DayStepResult<List<int>>();
        }

        public void SetNewSimulation()
        {
            if (!_logOnlyPopulation)
            {
                CreatureResults.Clear();
                TerrainResults.Clear();
            }
            PopulationResults.Clear();

            CurrentPopulation.ClearCreatureTypesCount();
            CurrentPopulation = new Population(_initialPopulation);
        }

        public SingleSimulationResults RunSimulation(int foodPerDay, PositionType positionType)
        {
            for (int day = 0; day < _simulationDays; day++)
            {
                CurrentPopulation.UpdateCreatures();
                if (CurrentPopulation.CreatureCount == 0)
                {
                    Log.Information($"All the creatures died on day {day - 1}, so no more days will be simulated.");
                    break;
                }
                SetupDayStart(foodPerDay, positionType, day);

                int step = 0;
                while(step < _stepsPerDay && CurrentPopulation.CreaturesHaveEnergy())
                {
                    SimulateStep(day, CurrentTerrain.X, CurrentTerrain.Y);
                    step++;
                }

                StorePopulation(day);

                DetermineCreaturesNextStatus();
                PrintCreatures(CurrentPopulation.Creatures, day, false);
                PrintCreaturesNextStatus(CurrentPopulation.Creatures);
            }

            return new SingleSimulationResults(CreatureResults, TerrainResults, PopulationResults, _logOnlyPopulation);
        }

        private void StorePopulation(int day)
        {
            var population = new List<int>();
            foreach (ICreatureType creatureType in CurrentPopulation.CreatureTypes)
            {
                population.Add(creatureType.NumberOfCreatures);
            }
            PopulationResults.AddStep(population, day);
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

                Log.Information($"Creature ID#{creature.Id} collected {creature.FoodCollectedLastDay} food{phraseAboutHealth} So, next day, it will {nextStatusReadable}.");
            }
            Log.Information("");
        }

        private void DetermineCreaturesNextStatus()
        {
            CurrentPopulation.DetermineCreaturesNextStatus(FoodToSurvive, FoodToReproduce);
        }

        private void DetermineStepActions()
        {
            CurrentPopulation.CreaturesCheckSurroundings(CurrentTerrain.FoodUnits);
            CurrentPopulation.CheckCreaturesInteractions(CurrentTerrain.FoodUnits);
            CurrentTerrain.RemoveEatenFood();
        }

        private void SetupDayStart(int foodPerDay, PositionType positionType, int day)
        {
            CurrentPopulation.ResetCreatures();
            CurrentPopulation.SetPositions(positionType, CurrentTerrain.X, CurrentTerrain.Y);
            CurrentTerrain.ClearFood();
            CurrentTerrain.AddRandomFood(foodPerDay);
            PrintCreatures(CurrentPopulation.Creatures, day, true);
            PrintPopulations(day, true);
        }

        private void PrintCreatures(CreatureList currentCreatures, int day, bool beginningOfDay)
        {
            Log.Information($"Creatures {(beginningOfDay ? "that start" : "at the end of")} day {day}:");
            foreach (Creature creature in currentCreatures)
            {
                Log.Information($"Creature ID#{creature.Id}, Health {creature.Health} ({creature.CreatureType.Name})");
            }
            Log.Information("");
        }

        public void PrintPopulations(int day, bool beginningOfDay)
        {
            Log.Information($"Population at the {(beginningOfDay ? "start" : "end")} of day {day}:");
            foreach (ICreatureType creatureType in CurrentPopulation.CreatureTypes)
            {
                Log.Information($"Number of creatures: {creatureType.NumberOfCreatures}");
            }
        }

        private void SimulateStep(int day, double xLimit, double yLimit)
        {
            foreach (Creature creature in CurrentPopulation.Creatures)
            {
                Log.Information($"Creature ID#{creature.Id} position X:{creature.Position.X:N2}, Y:{creature.Position.Y:N2}");
            }
            CurrentPopulation.MoveCreatures(xLimit, yLimit);
            if(!_logOnlyPopulation) StoreStepResults(day);
            DetermineStepActions();
        }

        private void StoreStepResults(int day)
        {
            CreatureResults.AddStep(new CreatureListDTO(CurrentPopulation.Creatures), day);
            TerrainResults.AddStep(new Terrain(CurrentTerrain), day);
        }
    }
}