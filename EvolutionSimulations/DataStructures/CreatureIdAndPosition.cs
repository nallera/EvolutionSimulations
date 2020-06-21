namespace EvolutionSimulations
{
    public class CreatureIdAndPosition
    {
        public int CreatureId;
        public Coordinate Position;

        public CreatureIdAndPosition()
        {
            CreatureId = -1;
            Position = new Coordinate(-1.0, -1.0);
        }
    }
}