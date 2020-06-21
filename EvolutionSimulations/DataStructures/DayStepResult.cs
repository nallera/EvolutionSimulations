using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EvolutionSimulations
{
    public class DayStepResult<T>
    {
        public List<List<T>> data;

        public DayStepResult()
        {
            data = new List<List<T>>();
        }

        public void AddStep(T stepData, int day)
        {
            if (day >= data.Count)
            {
                data.Add(new List<T>());
            }
            data[day].Add(stepData);
        }

        public T GetDayStep(int day, int step)
        {
            return data[day][step];
        }

        public List<T> GetDay(int day)
        {
            return data[day];
        }
    }
}