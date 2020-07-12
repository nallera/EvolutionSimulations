using System.Collections;
using System.Collections.Generic;

namespace EvolutionSimulations
{
    public interface ICreatureType
    { 
        public string Name { get; }
        public double Health { get; }
        public double AttackPower { get; }
        public double MaxSpeed { get; }
        public double Reach { get; }
        public double Energy { get; }
        public bool IsHostile { get; }
        public bool IsPossibleMutation { get; }
        public int NumberOfCreatures { get; }

        public double EnergySpentInFight { get; }
        public double TakeDamageFromFight(Creature oponentCreature);
        public void AddCreature();
        public void RemoveCreature();
        public void ClearNumberOfCreatures();
    }
}