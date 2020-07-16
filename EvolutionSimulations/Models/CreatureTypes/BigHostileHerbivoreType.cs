using System;
using System.Collections.Generic;
using System.Text;

namespace EvolutionSimulations.Models.CreatureTypes
{
    class BigHostileHerbivoreType : ICreatureType
    {
        public string Name => "BigHostileHerbivore";
        public double Health => 100.0;
        public double AttackPower => 40.0;
        public double MaxSpeed => 1.0;
        public double Reach => 2.0;
        public double Energy => 40.0;
        public double ReachEnergyMultiplier => 2.0;
        public double SpeedEnergyMultiplier => 2.0;
        public double EnergySpentInFight => 15.0;
        public bool IsHostile => true;
        public bool IsHerbivore => true;
        public bool IsCarnivore => false;
        public int NumberOfCreatures { get; private set; }

        public BigHostileHerbivoreType()
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
            return oponentCreature.AttackPower;
        }

        public void ClearNumberOfCreatures()
        {
            NumberOfCreatures = 0;
        }
    }
}
