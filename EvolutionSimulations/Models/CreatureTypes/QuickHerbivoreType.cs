using System;
using System.Collections.Generic;
using System.Text;

namespace EvolutionSimulations.Models.CreatureTypes
{
    class QuickHerbivoreType : ICreatureType
    {
        public string Name => "QuickHerbivore";
        public double Health => 100.0;
        public double AttackPower => 10.0;
        public double MaxSpeed => 2.0;
        public double Reach => 1.0;
        public double Energy => 40.0;
        public double ReachEnergyMultiplier => 1.0;
        public double SpeedEnergyMultiplier => 2.0;
        public double EnergySpentInFight => 10.0;
        public bool IsHostile => false;
        public bool IsHerbivore => true;
        public bool IsCarnivore => false;
        public int NumberOfCreatures { get; private set; }

        public QuickHerbivoreType()
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
