using System;
using System.Collections.Generic;

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

        public ICreatureType CreatureType { get; set; }

        public double FoodCollected;
        public double FoodCollectedLastDay;
        public LifeStatus NextStatus;

        public Coordinate Position;

        public List<int> FoodInReachIds;
        public List<int> CreaturesInReachIds;
        public List<int> ReachingCreaturesIds;


        public Creature(Creature source)
        {
            Id = source.Id;

            Health = source.Health;
            AttackPower = source.AttackPower;
            MaxSpeed = source.MaxSpeed;
            Reach = source.Reach;
            CreatureType = source.CreatureType;

            FoodCollected = 0;
            FoodCollectedLastDay = 0;
            NextStatus = LifeStatus.StayAlive;

            _randomGen = new Random(Guid.NewGuid().GetHashCode());

            Position = new Coordinate(source.Position);

            FoodInReachIds = new List<int>(source.FoodInReachIds);
            CreaturesInReachIds = new List<int>(source.CreaturesInReachIds);
            ReachingCreaturesIds = new List<int>(source.ReachingCreaturesIds);
        }

        public Creature(int id, ICreatureType creatureType)
        {
            Id = id;

            CreatureType = creatureType;

            Health = creatureType.Health;
            AttackPower = creatureType.AttackPower;
            MaxSpeed = creatureType.MaxSpeed;
            Reach = creatureType.Reach;
            Energy = creatureType.Energy;

            FoodCollected = 0;
            FoodCollectedLastDay = 0;
            NextStatus = LifeStatus.StayAlive;

            _randomGen = new Random(Guid.NewGuid().GetHashCode());

            Position = new Coordinate(0.0, 0.0);

            FoodInReachIds = new List<int>();
            CreaturesInReachIds = new List<int>();
            ReachingCreaturesIds = new List<int>();
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

            Energy -= speed * CreatureType.SpeedEnergyMultiplier;
        }

        internal void CollectFood(double foodAmount)
        {
            FoodCollected += foodAmount;
            Energy -= Math.PI * Math.Pow(Reach,2) * CreatureType.ReachEnergyMultiplier;
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

        internal bool HasEnergy()
        {
            return Energy > 0;
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

        public void SpendEnergyInFight()
        {
            Energy -= CreatureType.EnergySpentInFight;
        }

        public double TakeDamageFromFight(Creature creature)
        {
            double damage = CreatureType.TakeDamageFromFight(creature);
            Health -= damage;

            return damage;
        }

        internal void CheckSurroundings(CreatureList creatures, List<Food> food)
        {
            if (CreatureType.IsHerbivore) FoodInReachIds = CheckFoodInReach(food);
            CreaturesInReachIds = CheckCreaturesInReach(creatures);
        }

        internal void ResetSurroundingsFindings()
        {
            FoodInReachIds.Clear();
            CreaturesInReachIds.Clear();
            ReachingCreaturesIds.Clear();
        }

        public List<int> CheckCreaturesInReach(CreatureList creatures)
        {
            List<int> creaturesInReach = new List<int>();

            foreach (Creature creature in creatures)
            {
                if (creature.Id != Id && Position.InReach(creature.Position, Reach))
                {
                    creaturesInReach.Add(creature.Id);
                    creature.ReachingCreaturesIds.Add(Id);
                }
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
            Health = CreatureType.Health;
            Energy = CreatureType.Energy;
        }
    }
}
