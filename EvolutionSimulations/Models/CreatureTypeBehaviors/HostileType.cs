using System;
using System.Collections.Generic;
using System.Text;

namespace EvolutionSimulations.Models.CreatureTypes
{
    class HostileType : ICreatureType
    {
        public string Name => "Hostile";
        public double Health => 100.0;
        public double AttackPower => 20.0;
        public double MaxSpeed => 1.0;
        public double Reach => 1.0;
        public double Energy => 40.0;
        public bool IsHostile => true;
        public int NumberOfCreatures { get; private set; }

        public HostileType()
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
            return oponentCreature.AttackPower;
        }

        public void ClearNumberOfCreatures()
        {
            NumberOfCreatures = 0;
        }
    }
}