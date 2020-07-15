using EvolutionSimulations.Models.CreatureTypes;
using Serilog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace EvolutionSimulations
{
    public class CreatureTypeList : IEnumerable<ICreatureType>
    {
        List<ICreatureType> CreatureTypes;
        public List<DoubleRange> MutationTypeRanges;
        Random RandomGen;

        public CreatureTypeList()
        {
            CreatureTypes = new List<ICreatureType>();
            MutationTypeRanges = new List<DoubleRange>();
            RandomGen = new Random();
        }

        public ICreatureType this[int index]
        {
            get
            {
                return CreatureTypes[index];
            }
            set
            {
                CreatureTypes[index] = value;
            }
        }

        public void Add(ICreatureType creatureType)
        {
            CreatureTypes.Add(creatureType);
            if(MutationTypeRanges.Count != 0)
                MutationTypeRanges.Add(new DoubleRange(MutationTypeRanges.Last().End, MutationTypeRanges.Last().End));
            else
                MutationTypeRanges.Add(new DoubleRange(0.0, 0.0));
        }

        public void Add(ICreatureType creatureType, double mutationProbability)
        {
            CreatureTypes.Add(creatureType);

            if (MutationTypeRanges.Count != 0)
                MutationTypeRanges.Add(new DoubleRange(MutationTypeRanges.Last().End, MutationTypeRanges.Last().End + mutationProbability));
            else
                MutationTypeRanges.Add(new DoubleRange(0.0, mutationProbability));
        }

        public void Clear()
        {
            CreatureTypes.Clear();
        }

        public ICreatureType Mutate(ICreatureType creatureType)
        {
            ICreatureType MutationResult = creatureType;

            double value = RandomGen.NextDouble();

            for(int index = 0; index < MutationTypeRanges.Count; index++)
            {
                if (MutationTypeRanges[index].IsInRange(value) && CreatureTypes[index] != creatureType)
                {
                    MutationResult = CreatureTypes[index];
                    Log.Information($"A mutation occured! A {creatureType.Name} creature mutated into a {MutationResult.Name} creature.");
                    break;
                }
            }

            return MutationResult;
        }

        public IEnumerator<ICreatureType> GetEnumerator()
        {
            return CreatureTypes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}