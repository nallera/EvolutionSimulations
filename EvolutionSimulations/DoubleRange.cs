namespace EvolutionSimulations
{
    public class DoubleRange
    {

        public double Start { get; set; }
        public double End { get; set; }

        public DoubleRange(double start, double end)
        {
            Start = start;
            End = end;
        }

        public bool IsInRange(double value)
        {
            return (value > Start && value < End);
        }
    }
}