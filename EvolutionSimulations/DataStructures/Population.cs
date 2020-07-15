using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EvolutionSimulations
{
    public class Population
    {
        public CreatureTypeList CreatureTypes { get; set; }
        public CreatureList Creatures;
        public int CreatureCount { get { return Creatures.Count; } }

        public Population(CreatureTypeList creatureTypes)
        {
            CreatureTypes = creatureTypes;
            Creatures = new CreatureList();
        }

        public Population(Population source)
        {
            CreatureTypes = source.CreatureTypes;
            Creatures = new CreatureList(source.Creatures);
        }

        public void Clear()
        {
            CreatureTypes.Clear();
            Creatures.Clear();
        }

        public ICreatureType this[int index]
        {
            get
            {
                return CreatureTypes[index];
            }
            set
            {
                CreatureTypes[index] = value;
            }
        }

        public void AddCreature(ICreatureType creatureType)
        {
            Creatures.AddCreature(creatureType);
        }
        public void RemoveCreature(Creature creature)
        {
            Creatures.RemoveCreature(creature.Id);
        }

        public void UpdateCreatures()
        {
            foreach (Creature reproducingCreature in Creatures.FindAll(creature => creature.NextStatus == LifeStatus.Reproduce))
            {
                ICreatureType newbornCreatureType = CreatureTypes.Mutate(reproducingCreature.CreatureType);
                AddCreature(newbornCreatureType);
            }

            foreach (Creature dyingCreature in Creatures.FindAll(creature => creature.NextStatus == LifeStatus.Die))
            {
                RemoveCreature(dyingCreature);
            }
        }

        public void DetermineCreaturesNextStatus(int foodToSurvive, int foodToReproduce)
        {
            Creatures.DetermineCreaturesNextStatus(foodToSurvive, foodToReproduce);
        }

        internal void ClearCreatureTypesCount()
        {
            foreach(ICreatureType creatureType in CreatureTypes)
            {
                creatureType.ClearNumberOfCreatures();
            }
        }

        internal void CountInitialCreatureTypes()
        {
            foreach (Creature creature in Creatures)
            {
                creature.CreatureType.AddCreature();
            }
        }

        internal bool CreaturesHaveEnergy()
        {
            return Creatures.HaveEnergy();
        }

        public void CreaturesCheckSurroundings(List<Food> food)
        {
            Creatures.CheckSurroundings(food);
        }

        public void CheckCreaturesInteractions(List<Food> food)
        {
            Creatures.CheckInteractions(food);
        }

        public void ResetCreatures()
        {
            Creatures.ResetCreatures();
        }

        public void SetPositions(PositionType positionType, double xLimit, double yLimit)
        {
            Creatures.SetPositions(positionType, xLimit, yLimit);
        }

        public void MoveCreatures(double xLimit, double yLimit)
        {
            Creatures.MoveCreatures(xLimit, yLimit);
        }
    }
}