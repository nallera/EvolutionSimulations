using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EvolutionSimulations
{
    public class DayStepResult<T>
    {
        public List<List<T>> results;
        private DayStepResult<List<int>> populationResults;

        public DayStepResult()
        {
            results = new List<List<T>>();
        }

        public DayStepResult(DayStepResult<T> source)
        {
            results = source.results.ConvertAll(result => new List<T>(result));
        }

        public void AddStep(T stepData, int day)
        {
            if (day >= results.Count)
            {
                results.Add(new List<T>());
            }
            results[day].Add(stepData);
        }

        public T GetDayStep(int day, int step)
        {
            return results[day][step];
        }

        public List<T> GetDay(int day)
        {
            return results[day];
        }

        public void Clear()
        {
            results.Clear();
        }
    }
}