using System.Collections.Generic;

namespace EvolutionSimulations
{
    public class SimulationParameters
    {
        public int xLimit;
        public int yLimit;
        public int simulationDays;
        public int stepsPerDay;
        public int foodPerDay;
        public int foodToSurvive;
        public int foodToReproduce;
        public int numberOfSimulations;
        public bool logOnlyPopulation;

        public List<string> CreatureTypes;

        public SimulationParameters()
        {
            CreatureTypes = new List<string>();
        }
    }
}