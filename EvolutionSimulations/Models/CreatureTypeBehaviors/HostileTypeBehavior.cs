using System;
using System.Collections.Generic;
using System.Text;

namespace EvolutionSimulations.Models.CreatureTypes
{
    class HostileTypeBehavior : ICreatureTypeBehavior
    {
        public double Health => 100.0;

        public double AttackPower => 20.0;

        public double MaxSpeed => 1.0;

        public double Reach => 1.0;

        public double Energy => 40.0;

        public bool IsHostile => true;


        double ICreatureTypeBehavior.EnergySpentInFight => 20.0;

        public double TakeDamageFromFight(Creature oponentCreature)
        {
            return oponentCreature.AttackPower;
        }
    }
}