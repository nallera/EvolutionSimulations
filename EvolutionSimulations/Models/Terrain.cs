using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace EvolutionSimulations
{
    public class Terrain
    {
        public double X;
        public double Y;
        public List<Food> FoodUnits;

        public Terrain(Terrain source)
        {
            X = source.X;
            Y = source.Y;

            FoodUnits = new List<Food>(source.FoodUnits);
        }

        public Terrain(double x, double y)
        {
            X = x;
            Y = y;

            FoodUnits = new List<Food>();
        }

        public void AddRandomFood(int foodAmount)
        {
            Random randomGen = new Random(Guid.NewGuid().GetHashCode());
            int placedFoodCount = 0;

            while (placedFoodCount < foodAmount)
            {
                double xFood = randomGen.NextDouble() * X;
                double yFood = randomGen.NextDouble() * Y;

                FoodUnits.Add(new Food(xFood,yFood));
                placedFoodCount++;
            }
        }

        internal void ClearFood()
        {
            FoodUnits.Clear();
        }

        internal void RemoveEatenFood()
        {
            FoodUnits.RemoveAll(f => f.Eaten);
        }

        public bool FoodRemains()
        {
            return (FoodUnits.Count > 0);
        }

        //public void PrintTerrain()
        //{
        //    foreach (var line in Cells)
        //    {
        //        foreach(var column in line)
        //        {
        //            if (column.HasFood())
        //            {
        //                if(column.CreatureCount > 0)
        //                {
        //                    Console.Write("X"); 
        //                }
        //                else
        //                {
        //                    Console.Write("F");
        //                }
        //            }
        //            else
        //            {
        //                Console.Write(column.CreatureCount);
        //            }
        //        }
        //        Console.Write("\n");
        //    }
        //}

        //public void UpdateCreaturePositions(List<CreatureIdAndPosition> creaturePositions)
        //{
        //    ClearCreaturePositions();

        //    foreach (var position in creaturePositions)
        //    {
        //        Cells[position.XPosition][position.YPosition].AddCreature(position.CreatureId);
        //    }
        //}

        //public void ClearCreaturePositions()
        //{
        //    foreach (var line in Cells)
        //    {
        //        foreach (var cell in line)
        //        {
        //            cell.ClearCreatures();
        //        }
        //    }
        //}

        //public List<CreatureInteraction> CellsOutcome()
        //{
        //    List<CreatureInteraction> interactions = new List<CreatureInteraction>();

        //    foreach(var line in Cells)
        //    {
        //        foreach(var cell in line)
        //        {
        //            if((cell.CreatureCount > 0 && cell.HasFood()) || (cell.CreatureCount > 1))
        //            {
        //                interactions.Add(new CreatureInteraction(cell.GetCreatures(), cell.HasFood()));
        //                cell.ClearFood();
        //            }
        //        }
        //    }

        //    return interactions;
        //}
    }
}