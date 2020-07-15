using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace EvolutionSimulations.Models.CreatureTypes
{
    public class FriendlyType : ICreatureType
    {
        public string Name => "Friendly";
        public double Health => 100.0;
        public double AttackPower => 10.0;
        public double MaxSpeed => 1.0;
        public double Reach => 1.0;
        public double Energy => 40.0;
        public bool IsHostile => false;
        public int NumberOfCreatures { get; private set; }

        double ICreatureType.EnergySpentInFight => 10.0;

        public FriendlyType()
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
                return oponentCreature.AttackPower * 2;
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
