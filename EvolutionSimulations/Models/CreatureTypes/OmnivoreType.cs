using System;
using System.Collections.Generic;
using System.Text;

namespace EvolutionSimulations.Models.CreatureTypes
{
    class OmnivoreType : ICreatureType
    {
        public string Name => "Omnivore";
        public double Health => 100.0;
        public double AttackPower => 25.0;
        public double MaxSpeed => 1.0;
        public double Reach => 1.0;
        public double Energy => 40.0;
        public double ReachEnergyMultiplier => 1.0;
        public double SpeedEnergyMultiplier => 1.0;
        public double EnergySpentInFight => 10.0;
        public bool IsHostile => false;
        public bool IsHerbivore => true;
        public bool IsCarnivore => true;
        public int NumberOfCreatures { get; private set; }

        public OmnivoreType()
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
            if (oponentCreature.CreatureType.IsCarnivore) return oponentCreature.AttackPower * 2.5;
            else return oponentCreature.AttackPower;
        }

        public void ClearNumberOfCreatures()
        {
            NumberOfCreatures = 0;
        }
    }
}


