using System.Diagnostics;

namespace EvolutionSimulations
{
    public class CreatureCharacteristics
    {
        public double Health;
        public double AttackPower;
        public double MaxSpeed;
        public double Reach;
        public double Energy;

        public CreatureCharacteristics()
        {

        }

        public CreatureCharacteristics(CreatureCharacteristics source)
        {
            Health = source.Health;
            AttackPower = source.AttackPower;
            MaxSpeed = source.MaxSpeed;
            Reach = source.Reach;
            Energy = source.Energy;
        }
    }
}