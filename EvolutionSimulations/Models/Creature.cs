using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace EvolutionSimulations
{
    public class Creature
    {
        public int Id;

        private Random _randomGen;

        public double Health;
        public double AttackPower;
        public double MaxSpeed;
        public double Reach;
        public double Energy;

        public List<CreatureTrait> Traits { get; set; }

        public double FoodCollected;
        public double FoodCollectedLastDay;
        public LifeStatus NextStatus;

        public Coordinate Position;

        public List<int> FoodInReachIds;
        public List<int> CreaturesInReachIds;

        private readonly int _hostileAttackMultiplier = 4;
        private readonly double _attackEnergyCost = 10;

        public Creature(Creature source)
        {
            Id = source.Id;

            Health = source.Health;
            AttackPower = source.AttackPower;
            MaxSpeed = source.MaxSpeed;
            Reach = source.Reach;
            Traits = new List<CreatureTrait>(source.Traits);

            FoodCollected = 0;
            FoodCollectedLastDay = 0;
            NextStatus = LifeStatus.StayAlive;

            _randomGen = new Random(Guid.NewGuid().GetHashCode());

            Position = new Coordinate(source.Position);

            FoodInReachIds = new List<int>(source.FoodInReachIds);
            CreaturesInReachIds = new List<int>(source.CreaturesInReachIds);
        }

        public Creature(int id, List<CreatureTrait> traits, CreatureCharacteristics characteristics)
        {
            Id = id;

            Health = characteristics.Health;
            AttackPower = characteristics.AttackPower;
            MaxSpeed = characteristics.MaxSpeed;
            Reach = characteristics.Reach;
            Energy = characteristics.Energy;


            Traits = new List<CreatureTrait>(traits);

            FoodCollected = 0;
            FoodCollectedLastDay = 0;
            NextStatus = LifeStatus.StayAlive;

            _randomGen = new Random(Guid.NewGuid().GetHashCode());

            Position = new Coordinate(0.0, 0.0);

            FoodInReachIds = new List<int>();
            CreaturesInReachIds = new List<int>();
        }

        public void Move(double xLimit, double yLimit)
        {
            double angle, speed = 0.0, newXPosition = 0.0, newYPosition = 0.0;
            bool outOfLimits = true;

            while (outOfLimits)
            {
                angle = _randomGen.NextDouble() * 2 * Math.PI;
                speed = _randomGen.NextDouble() * MaxSpeed;

                newXPosition = Position.X + Math.Cos(angle) * speed;
                newYPosition = Position.Y + Math.Cos(angle) * speed;

                outOfLimits = (newXPosition < 0.0 || newXPosition > xLimit || newYPosition < 0.0 || newYPosition > yLimit) ? true : false;
            }

            Position.X = newXPosition;
            Position.Y = newYPosition;

            Energy -= speed;
        }

        internal void SetPosition(PositionType positionType, double xLimit, double yLimit)
        {
            switch(positionType)
            {
                case PositionType.Random:

                    Position.X = _randomGen.NextDouble() * xLimit;
                    Position.Y = _randomGen.NextDouble() * yLimit;
                    break;

                case PositionType.Border:

                    switch (_randomGen.Next(2))
                    {
                        case 0:
                            Position.X = 1.0 + _randomGen.Next(2) * (xLimit - 2.0);
                            Position.Y = _randomGen.NextDouble() * yLimit;
                            break;
                        case 1:
                            Position.X = _randomGen.NextDouble() + xLimit;
                            Position.Y = 1.0 + _randomGen.Next(2) * (yLimit - 2.0);
                            break;
                    }
                    break;
            }
        }

        public void DetermineNextStatusAndClearFood(int foodToSurvive, int foodToReproduce)
        {
            if (Health <= 0.0) NextStatus = LifeStatus.Die;
            else
            {
                switch (FoodCollected)
                {
                    case var d when d < foodToSurvive:
                        NextStatus = LifeStatus.Die;
                        break;
                    case var d when d >= foodToSurvive && d < foodToReproduce:
                        NextStatus = LifeStatus.StayAlive;
                        break;
                    case var d when d >= foodToReproduce:
                        NextStatus = LifeStatus.Reproduce;
                        break;
                }
            }

            FoodCollectedLastDay = FoodCollected;
            FoodCollected = 0;
        }

        internal void SpendEnergyInFight()
        {
            switch(Traits)
            {
                case var t when t.Contains(CreatureTrait.Friendly):
                    Energy -= _attackEnergyCost;
                    break;
                case var t when t.Contains(CreatureTrait.Hostile):
                    Energy -= _attackEnergyCost * 2;
                    break;
            }
        }

        public double TakeDamageFromFight(Creature creature)
        {
            if (Traits.Contains(CreatureTrait.Friendly))
            {
                if (creature.Traits.Contains(CreatureTrait.Hostile))
                {
                    Health -= creature.AttackPower * _hostileAttackMultiplier;
                    return creature.AttackPower * _hostileAttackMultiplier;
                }
            }
            else if (Traits.Contains(CreatureTrait.Hostile))
            {
                if (creature.Traits.Contains(CreatureTrait.Hostile))
                {
                    Health -= creature.AttackPower;
                    return creature.AttackPower;
                }
                else if (creature.Traits.Contains(CreatureTrait.Friendly))
                {
                    Health -= creature.AttackPower;
                    return creature.AttackPower;
                }
            }

            return 0.0;
        }

        internal void CheckSurroundings(List<CreatureIdAndPosition> creatures, List<Food> food)
        {
            FoodInReachIds = CheckFoodInReach(food);
            CreaturesInReachIds = CheckCreaturesInReach(creatures);
        }

        internal void ResetSurroundingsFindings()
        {
            FoodInReachIds.Clear();
            CreaturesInReachIds.Clear();
        }

        public List<int> CheckCreaturesInReach(List<CreatureIdAndPosition> creatures)
        {
            List<int> creaturesInReach = new List<int>();

            foreach (CreatureIdAndPosition creature in creatures)
            {
                if (creature.CreatureId != Id && Position.InReach(creature.Position, Reach)) creaturesInReach.Add(creature.CreatureId);
            }

            return creaturesInReach;
        }

        public List<int> CheckFoodInReach(List<Food> food)
        {
            List<int> foodInReach = new List<int>();

            for(int index = 0; index < food.Count; index++)
            {
                if (Position.InReach(food[index].Position, Reach))
                {
                    foodInReach.Add(index);
                    food[index].ReachingCreatures.Add(Id);
                }
            }

            return foodInReach;
        }

        internal void Reset()
        {
            Health = 100.0;
            AttackPower = 10.0;
            MaxSpeed = 1.0;
            Reach = 1.0;
        }
    }
}
