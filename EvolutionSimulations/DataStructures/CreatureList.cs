using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
            creatures.Add(new Creature(sourceCreature) { Id = lastId});
            return creatures.Count;
        }

        public int AddNewCreature(int xLimit, int yLimit, List<CreatureTreat> treats)
        {
            lastId++;
            creatures.Add(new Creature(lastId, xLimit, yLimit, treats));

            return creatures.Count;
        }

        public void RemoveCreature(int id)
        {
            creatures.RemoveAt(creatures.FindIndex(creature => creature.Id == id));
        }

        internal void SetPositions(PositionType positionType)
        {
            foreach (Creature creature in creatures)
            {
                creature.SetPosition(positionType);
            }
        }

        public List<CreatureIdAndPosition> GetPositions()
        {
            List<CreatureIdAndPosition> positions = new List<CreatureIdAndPosition>();

            foreach (Creature creature in creatures)
            {
                positions.Add(new CreatureIdAndPosition { CreatureId = creature.Id, 
                                                          XPosition = creature.XPosition, 
                                                          YPosition = creature.YPosition });
            }

            return positions;
        }

        internal void UpdateCreatures()
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

        internal void ProcessInteractions(List<CreatureInteraction> interactions)
        {
            foreach (var interaction in interactions)
            {
                if(interaction.FoodInvolved())
                {
                    foreach(var id in interaction.GetIds())
                    {
                        creatures[creatures.FindIndex(creature => creature.Id == id)].FoodCollected += 1 / interaction.CreaturesCount;
                    }
                }
            }
        }

        internal void Reset()
        {
            foreach (Creature creature in creatures)
            {
                creature.Reset();
            }
        }

        internal void Move()
        {
            foreach (Creature creature in creatures)
            {
                creature.Move();
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