using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EvolutionSimulations
{
    public class Population
    {
        public List<CreatureType> CreatureTypes { get; set; }
        public CreatureList Creatures;
        public int CreatureCount { get { return Creatures.Count; } }

        private CreatureCharacteristics _baseCharacteristics;

        //public Population()
        //{
        //    CreatureTypes = new List<CreatureType>();
        //    Creatures = new CreatureList();
        //}
        public Population(List<CreatureType> creatureTypes, CreatureCharacteristics characteristics)
        {
            CreatureTypes = new List<CreatureType>(creatureTypes);
            Creatures = new CreatureList();
            _baseCharacteristics = new CreatureCharacteristics(characteristics);
        }

        public Population(Population source)
        {
            CreatureTypes = source.CreatureTypes.ConvertAll(creatureType => new CreatureType(creatureType));
            Creatures = new CreatureList(source.Creatures);
            _baseCharacteristics = new CreatureCharacteristics(source._baseCharacteristics);
        }

        public void Clear()
        {
            CreatureTypes.Clear();
            Creatures.Clear();
        }

        public CreatureType this[int index]
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

        public void AddNewCreature(CreatureType creatureType)
        {
            Creatures.AddNewCreature(creatureType, _baseCharacteristics);
            CreatureAdded(creatureType.Traits);
        }
        public void AddCopyCreature(Creature creature)
        {
            Creatures.AddCopyCreature(creature);
            CreatureAdded(creature.Traits);
        }
        public void RemoveCreature(Creature creature)
        {
            Creatures.RemoveCreature(creature.Id);
            CreatureRemoved(creature.Traits);
        }

        public void UpdateCreatures()
        {
            foreach (Creature reproducingCreature in Creatures.FindAll(creature => creature.NextStatus == LifeStatus.Reproduce))
            {
                AddCopyCreature(reproducingCreature);
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

        public void CreatureAdded(List<CreatureTrait> Traits)
        {
            int index = CreatureTypes.FindIndex(creatureType => creatureType.Traits.SequenceEqual(Traits));
            CreatureTypes[index].NumberOfCreatures++;
        }

        public void CreatureRemoved(List<CreatureTrait> Traits)
        {
            int index = CreatureTypes.FindIndex(creatureType => creatureType.Traits.SequenceEqual(Traits));
            CreatureTypes[index].NumberOfCreatures--;
        }
    }
}