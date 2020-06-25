using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvolutionSimulations.Extension
{
    public static class ExtensionMethods
    {
        public static T Clone<T>(this T source)
        {
            var jsonString = JsonConvert.SerializeObject(source);
            return (T)JsonConvert.DeserializeObject(jsonString);
        }
    }
}
