using System;
using System.Collections.Generic;
using System.Text;

namespace EvolutionSimulations.Models.CreatureTypes
{
    public class FriendlyTypeBehavior : ICreatureTypeBehavior
    {
        public double Health => 100.0;

        public double AttackPower => 10.0;

        public double MaxSpeed => 1.0;

        public double Reach => 1.0;

        public double Energy => 40.0;

        public bool IsHostile => false;


        double ICreatureTypeBehavior.EnergySpentInFight => 10.0;

        public double TakeDamageFromFight(Creature oponentCreature)
        {
            if (oponentCreature.CreatureTypeBehavior.IsHostile)
            {
                return oponentCreature.AttackPower * 2;
            }
            else
            {
                return 0.0;
            }
        }
    }
}
