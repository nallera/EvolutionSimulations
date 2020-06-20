using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EvolutionSimulations
{
    internal class DayStepResult<T>
    {
        private T[][] data;

        public DayStepResult(int days, int steps)
        {
            data = new T[days][];
            for(int i = 0; i < days; i++)
            {
                data[i] = new T[steps];
            }
        }

        public void AddStep(T stepData, int day, int step)
        {
            data[day][step] = stepData;
        }

        public T GetDayStep(int day, int step)
        {
            return data[day][step];
        }

        public T[] GetDay(int day)
        {
            return data[day];
        }
    }
}