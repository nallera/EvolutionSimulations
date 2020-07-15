using System;
using System.Collections.Generic;
using System.Text;

namespace EvolutionSimulations.Models.CreatureTypes
{
    class BigType : ICreatureType
    {
        public string Name => "Big";
        public double Health => 100.0;
        public double AttackPower => 20.0;
        public double MaxSpeed => 1.0;
        public double Reach => 2.0;
        public double Energy => 40.0;
        public double ReachEnergyMultiplier => 2.0;
        public double SpeedEnergyMultiplier => 2.0;
        public bool IsHostile => false;
        public int NumberOfCreatures { get; private set; }

        public BigType()
        {
            NumberOfCreatures = 0;
        }

        public void AddCreature()
        {
            NumberOfCreatures++;
        }

        public void RemoveCreature()
        {
            NumberOfCreatures--;
        }

        double ICreatureType.EnergySpentInFight => 20.0;

        public double TakeDamageFromFight(Creature oponentCreature)
        {
            if (oponentCreature.CreatureType.IsHostile)
            {
                return oponentCreature.AttackPower * 0.5;
            }
            else
            {
                return 0.0;
            }
        }

        public void ClearNumberOfCreatures()
        {
            NumberOfCreatures = 0;
        }
    }
}
