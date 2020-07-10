using System.Collections;
using System.Collections.Generic;

namespace EvolutionSimulations
{
    public interface ICreatureTypeBehavior
    { 
        public double Health { get; }
        public double AttackPower { get; }
        public double MaxSpeed { get; }
        public double Reach { get; }
        public double Energy { get; }
        public bool IsHostile { get; }


        public double EnergySpentInFight { get; }
        public double TakeDamageFromFight(Creature oponentCreature);
    }
}