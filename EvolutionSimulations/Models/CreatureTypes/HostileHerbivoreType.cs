using System;
using System.Collections.Generic;
using System.Text;

namespace EvolutionSimulations.Models.CreatureTypes
{
    class HostileHerbivoreType : ICreatureType
    {
        public string Name => "HostileHerbivore";
        public double Health => 100.0;
        public double AttackPower => 20.0;
        public double MaxSpeed => 1.0;
        public double Reach => 1.0;
        public double Energy => 40.0;
        public bool IsHostile => true;
        public bool IsHerbivore => true;
        public bool IsCarnivore => false;
        public int NumberOfCreatures { get; private set; }
        public double ReachEnergyMultiplier => 1.0;
        public double SpeedEnergyMultiplier => 1.0;
        public double EnergySpentInFight => 10.0;

        public HostileHerbivoreType()
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