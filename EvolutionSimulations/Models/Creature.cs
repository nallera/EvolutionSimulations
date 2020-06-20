using System;
using System.Collections.Generic;
using System.Text;

namespace EvolutionSimulations
{
    public class Creature
    {
        private Random _positionRandomGen;
        private Random _movementRandomGen;
        private readonly int _xLimit;
        private readonly int _yLimit;
        public int Health;
        public int AttackPower;
        public List<CreatureTreat> Treats { get; set; }
        public double FoodCollected;
        public double FoodCollectedLastDay;
        public LifeStatus NextStatus;

        public int XPosition;
        public int YPosition;
        public int Id;

        public Creature(Creature source)
        {
            Id = source.Id;

            Health = source.Health;
            AttackPower = source.AttackPower;
            Treats = source.Treats;

            FoodCollected = 0;
            FoodCollectedLastDay = 0;
            NextStatus = LifeStatus.StayAlive;

            _xLimit = source._xLimit;
            _yLimit = source._yLimit;
            _positionRandomGen = new Random(Guid.NewGuid().GetHashCode());
            _movementRandomGen = new Random(Guid.NewGuid().GetHashCode());

            XPosition = source.XPosition;
            YPosition = source.YPosition;
        }

        public Creature(int id, int xLimit, int yLimit, List<CreatureTreat> treats)
        {
            Id = id;

            Health = 100;
            AttackPower = 10;
            Treats = treats;

            FoodCollected = 0;
            FoodCollectedLastDay = 0;
            NextStatus = LifeStatus.StayAlive;

            _xLimit = xLimit;
            _yLimit = yLimit;
            _positionRandomGen = new Random(Guid.NewGuid().GetHashCode());
            _movementRandomGen = new Random(Guid.NewGuid().GetHashCode());

            XPosition = 0;
            YPosition = 0;
        }

        public void Move()
        {
            XPosition += GenerateMove(XPosition,_xLimit);
            YPosition += GenerateMove(YPosition,_yLimit);
        }

        private int GenerateMove(int actualPosition, int limit)
        {
            int Move;
            if (actualPosition == limit - 1)
            {
                Move = _movementRandomGen.Next(2) - 1;
            }
            else if (actualPosition == 0)
            {
                Move = _movementRandomGen.Next(2);
            }
            else
            {
                Move = _movementRandomGen.Next(3) - 1;
            }

            return Move;
        }

        internal void SetPosition(PositionType positionType)
        {
            switch(positionType)
            {
                case PositionType.Random:

                    XPosition = _positionRandomGen.Next(_xLimit);
                    YPosition = _positionRandomGen.Next(_yLimit);
                    break;

                case PositionType.Border:

                    switch (_positionRandomGen.Next(2))
                    {
                        case 0:
                            XPosition = (_xLimit - 1) * Convert.ToInt32(_positionRandomGen.Next(2));
                            YPosition = _positionRandomGen.Next(_yLimit);
                            break;
                        case 1:
                            XPosition = _positionRandomGen.Next(_xLimit);
                            YPosition =  (_yLimit - 1) * Convert.ToInt32(_positionRandomGen.Next(2));
                            break;
                    }
                    break;
            }
        }

        public void DetermineNextStatusAndClearFood(int foodToSurvive, int foodToReproduce)
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

            FoodCollectedLastDay = FoodCollected;
            FoodCollected = 0;
        }

        internal void Reset()
        {
            Health = 100;
            AttackPower = 10;
        }
    }
}
