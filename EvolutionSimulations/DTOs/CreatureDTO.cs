using System.Collections.Generic;

namespace EvolutionSimulations.DTOs
{
    public class CreatureDTO
    {
        public int Id;
        public double Health;
        public List<CreatureTrait> Traits { get; set; }

        public double FoodCollected;
        public LifeStatus NextStatus;

        public Coordinate Position;

        public CreatureDTO(Creature source)
        {
            Id = source.Id;
            Health = source.Health;
            Traits = new List<CreatureTrait>(source.Traits);
            FoodCollected = source.FoodCollected;
            NextStatus = source.NextStatus;
            Position = new Coordinate(source.Position);
        }
    }
}