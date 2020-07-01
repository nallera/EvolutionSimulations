using System;
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
        
        public int AddCopyCreature(Creature sourceCreature)
        {
            lastId++;
            OnCreatureAdded(new CreatureTraitsEventArgs { Traits = sourceCreature.Traits});
            creatures.Add(new Creature(sourceCreature) { Id = lastId});
            return creatures.Count;
        }

        public int AddNewCreature(CreatureType creatureType)
        {
            lastId++;
            OnCreatureAdded(new CreatureTraitsEventArgs { Traits = creatureType.Traits });
            creatures.Add(new Creature(lastId, creatureType.Traits));

            return creatures.Count;
        }

        public void RemoveCreature(int id)
        {
            OnCreatureRemoved(new CreatureTraitsEventArgs { Traits = creatures.Find(creature => creature.Id == id).Traits });
            creatures.RemoveAt(creatures.FindIndex(creature => creature.Id == id));
        }

        internal void SetPositions(PositionType positionType, double xLimit, double yLimit)
        {
            foreach (Creature creature in creatures)
            {
                creature.SetPosition(positionType, xLimit, yLimit);
            }
        }

        public void UpdateCreatures()
        {
            foreach (Creature reproducingCreature in creatures.FindAll(creature => creature.NextStatus == LifeStatus.Reproduce))
            {
                AddCopyCreature(reproducingCreature);
            }

            foreach (Creature dyingCreatures in creatures.FindAll(creature => creature.NextStatus == LifeStatus.Die))
            {
                RemoveCreature(dyingCreatures.Id);
            }
        }

        public List<CreatureIdAndPosition> GetPositions()
        {
            List<CreatureIdAndPosition> positions = new List<CreatureIdAndPosition>();

            foreach (Creature creature in creatures)
            {
                positions.Add(new CreatureIdAndPosition { CreatureId = creature.Id, 
                                                            Position = new Coordinate(creature.Position) });
            }

            return positions;
        }

        //internal void ProcessInteractions(List<CreatureInteraction> interactions)
        //{
        //    foreach (var interaction in interactions)
        //    {
        //        if(interaction.FoodInvolved())
        //        {
        //            foreach(var id in interaction.GetIds())
        //            {
        //                creatures[creatures.FindIndex(creature => creature.Id == id)].FoodCollected += 1 / interaction.CreaturesCount;
        //            }
        //        }
        //    }
        //}

        public void CheckSurroundings(List<Food> food)
        {
            ResetSurroundingsFindings();

            foreach (Creature creature in creatures)
            {
                creature.CheckSurroundings(GetPositions(), food);
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
                        creature.FoodCollected += 1.0 / foodUnits[foodId].ReachingCreatures.Count;
                        foodUnits[foodId].Eaten = true;
                        Log.Information($"Creature ID#{creature.Id} collected {1.0 / foodUnits[foodId].ReachingCreatures.Count:N2} food " +
                            $"from food ID#{foodId}, {foodUnits[foodId].ReachingCreatures.Count} creature(s) ate from this food.");
                    }
                }
                if (creature.CreaturesInReachIds.Count != 0)
                {
                    foreach (int creatureInReachId in creature.CreaturesInReachIds)
                    {
                        double damageTaken = creatures.Find(c => c.Id == creatureInReachId).TakeDamageFromFight(creature);
                        if (damageTaken != 0.0)
                        {
                            Log.Information($"Creature ID#{creature.Id} fought with creature ID#{creatureInReachId}, and caused it {damageTaken:N2} damage.");
                        }
                    }
                }
            }
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
                creature.Move(xLimit, yLimit);
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

        protected virtual void OnCreatureAdded(CreatureTraitsEventArgs e)
        {
            EventHandler<CreatureTraitsEventArgs> handler = CreatureAdded;
            handler?.Invoke(this, e);
        }
        protected virtual void OnCreatureRemoved(CreatureTraitsEventArgs e)
        {
            EventHandler<CreatureTraitsEventArgs> handler = CreatureRemoved;
            handler?.Invoke(this, e);
        }

        public event EventHandler<CreatureTraitsEventArgs> CreatureAdded;
        public event EventHandler<CreatureTraitsEventArgs> CreatureRemoved;
    }
}