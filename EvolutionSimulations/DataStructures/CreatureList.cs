﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Serilog;

namespace EvolutionSimulations
{
    public class CreatureList : IEnumerable<Creature>
    {
        private List<Creature> creatures;
        public int Count { get { return creatures.Count; } }
        private int lastId;

        public CreatureList(CreatureList source)
        {
            creatures = source.creatures.ConvertAll(creature => new Creature(creature));
            lastId = source.lastId;
        }
        public CreatureList()
        {
            creatures = new List<Creature>();
            lastId = -1;
        }

        public void Clear()
        {
            creatures.Clear();
            lastId = -1;
        }
        
        public int AddCreature(ICreatureType creatureType)
        {
            lastId++;
            creatures.Add(new Creature(lastId, creatureType));
            creatureType.AddCreature();

            return creatures.Count;
        }

        public void RemoveCreature(int id)
        {
            creatures.Find(creature => creature.Id == id).CreatureType.RemoveCreature();
            creatures.RemoveAt(creatures.FindIndex(creature => creature.Id == id));
        }

        public void SetPositions(PositionType positionType, double xLimit, double yLimit)
        {
            foreach (Creature creature in creatures)
            {
                creature.SetPosition(positionType, xLimit, yLimit);
            }
        }

        //public List<CreatureIdAndPositionandReachingCreatures> GetPositions()
        //{
        //    List<CreatureIdAndPositionandReachingCreatures> positions = new List<CreatureIdAndPositionandReachingCreatures>();

        //    foreach (Creature creature in creatures)
        //    {
        //        positions.Add(new CreatureIdAndPositionandReachingCreatures { CreatureId = creature.Id, 
        //                                                                    Position = new Coordinate(creature.Position),
        //                                                                    ReachingCreaturesIds = new List<int>(creature.ReachingCreaturesIds)});
        //    }

        //    return positions;
        //}

        public List<Creature> FindAll(Predicate<Creature> match)
        {
            return creatures.FindAll(match);
        }

        public void CheckSurroundings(List<Food> food)
        {
            ResetSurroundingsFindings();

            foreach (Creature creature in creatures)
            {
                creature.CheckSurroundings(this, food);
            }
        }

        internal void CheckInteractions(List<Food> foodUnits)
        {
            foreach (Creature creature in creatures)
            {
                if (creature.FoodInReachIds.Count != 0)
                {
                    foreach (int foodId in creature.FoodInReachIds)
                    {
                        if (creature.HasEnergy())
                        {
                            creature.CollectFood(1.0 / foodUnits[foodId].ReachingCreatures.Count);
                            foodUnits[foodId].Eaten = true;

                            Log.Information($"Creature ID#{creature.Id} collected {1.0 / foodUnits[foodId].ReachingCreatures.Count:N2} food " +
                                $"from food ID#{foodId}, {foodUnits[foodId].ReachingCreatures.Count} creature(s) ate from this food.");
                        }
                        else
                        {
                            //Log.Information($"Creature ID#{creature.Id} doesn't have enough energy to eat.");
                            break;
                        }
                    }
                }
                if (creature.CreaturesInReachIds.Count != 0)
                {
                    foreach (int creatureInReachId in creature.CreaturesInReachIds)
                    {
                        if (creature.HasEnergy())
                        {
                            double damageTaken = creatures.Find(c => c.Id == creatureInReachId).TakeDamageFromFight(creature);

                            if (damageTaken != 0.0) Log.Information($"Creature ID#{creature.Id} fought with creature ID#{creatureInReachId}, " +
                                $"and caused it {damageTaken:N2} damage.");

                            if (creature.CreatureType.IsCarnivore)
                            {
                                if (creatures.Find(c => c.Id == creatureInReachId).Health <= 0.0)
                                {
                                    double foodEaten = 4.0 / creatures.Find(c => c.Id == creatureInReachId).ReachingCreaturesIds.Count;
                                    creature.CollectFood(foodEaten);
                                    Log.Information($"Creature ID#{creature.Id} ate {foodEaten:N2} food from creature ID#{creatureInReachId}, " +
                                        $"which was killed.");
                                }
                                else
                                {
                                    double foodEaten = 1.0 / creatures.Find(c => c.Id == creatureInReachId).ReachingCreaturesIds.Count;
                                    creature.CollectFood(foodEaten);
                                    Log.Information($"Creature ID#{creature.Id} ate {foodEaten:N2} food from creature ID#{creatureInReachId}.");
                                }
                            }
                                

                            creature.SpendEnergyInFight();
                        }
                        else
                        {
                            //Log.Information($"Creature ID#{creature.Id} doesn't have enough energy to fight.");
                            break;
                        }
                    }
                }
            }
        }

        public bool HaveEnergy()
        {
            bool result = false;

            foreach (Creature creature in creatures)
            {
                if (creature.HasEnergy())
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public void ResetSurroundingsFindings()
        {
            foreach (Creature creature in creatures)
            {
                creature.ResetSurroundingsFindings();
            }
        }

        internal void ResetCreatures()
        {
            foreach (Creature creature in creatures)
            {
                creature.Reset();
            }
        }

        internal void MoveCreatures(double xLimit, double yLimit)
        {
            foreach (Creature creature in creatures)
            {
                if(creature.HasEnergy()) creature.Move(xLimit, yLimit);
                //else Log.Information($"Creature ID#{creature.Id} doesn't have enough energy to move.");
            }
        }

        public void DetermineCreaturesNextStatus(int foodToSurvive, int foodToReproduce)
        {
            foreach (var creature in creatures)
            {
                creature.DetermineNextStatusAndClearFood(foodToSurvive, foodToReproduce);
            }
        }

        public IEnumerator<Creature> GetEnumerator()
        {
            return creatures.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}