﻿using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace EvolutionSimulations.Models.CreatureTypes
{
    public class HerbivoreType : ICreatureType
    {
        public string Name => "Herbivore";
        public double Health => 100.0;
        public double AttackPower => 10.0;
        public double MaxSpeed => 1.0;
        public double Reach => 1.0;
        public double Energy => 40.0;
        public bool IsHostile => false;
        public bool IsHerbivore => true;
        public bool IsCarnivore => false;
        public int NumberOfCreatures { get; private set; }
        public double EnergySpentInFight => 5.0;
        public double ReachEnergyMultiplier => 1.0;
        public double SpeedEnergyMultiplier => 1.0;

        public HerbivoreType()
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
            if (oponentCreature.CreatureType.IsHostile) return oponentCreature.AttackPower;
            else return 0.0;

        }

        public void ClearNumberOfCreatures()
        {
            NumberOfCreatures = 0;
        }

    }
}
