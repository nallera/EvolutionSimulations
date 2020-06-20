using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace EvolutionSimulations
{
    public class Terrain
    {
        public int X;
        public int Y;
        public CellContent[][] Cells;

        public Terrain(Terrain source)
        {
            X = source.X;
            Y = source.Y;

            Cells = new CellContent[X][];
            for (int line = 0; line < X; line++)
            {
                Cells[line] = new CellContent[Y];
                for (int column = 0; column < Y; column++)
                {
                    Cells[line][column] = new CellContent(source.Cells[line][column]);
                }
            }
        }

        public Terrain(int x, int y)
        {
            X = x;
            Y = y;

            Cells = new CellContent[X][];
            for(int line = 0; line < X; line++)
            {
                Cells[line] = new CellContent[Y];
                for (int column = 0; column < Y; column++)
                {
                    Cells[line][column] = new CellContent();
                }
            }
        }

        public void AddRandomFood(int foodAmount)
        {
            Random randomGen = new Random(Guid.NewGuid().GetHashCode());
            int placedFoodCount = 0;

            while (placedFoodCount < foodAmount)
            {
                int xFood = randomGen.Next(X);
                int yFood = randomGen.Next(Y);

                if (!Cells[xFood][yFood].HasFood())
                {
                    Cells[xFood][yFood].AddFood();
                    placedFoodCount++;
                }
            }
        }

        internal void ClearFood()
        {
            foreach (var line in Cells)
            {
                foreach (var column in line)
                {
                    if (column.HasFood())
                    {
                        column.ClearFood();
                    }
                }
                Console.Write("\n");
            }
        }

        public void PrintTerrain()
        {
            foreach (var line in Cells)
            {
                foreach(var column in line)
                {
                    if (column.HasFood())
                    {
                        if(column.CreatureCount > 0)
                        {
                            Console.Write("X"); 
                        }
                        else
                        {
                            Console.Write("F");
                        }
                    }
                    else
                    {
                        Console.Write(column.CreatureCount);
                    }
                }
                Console.Write("\n");
            }
        }

        public void UpdateCreaturePositions(List<CreatureIdAndPosition> creaturePositions)
        {
            ClearCreaturePositions();

            foreach (var position in creaturePositions)
            {
                Cells[position.XPosition][position.YPosition].AddCreature(position.CreatureId);
            }
        }

        public void ClearCreaturePositions()
        {
            foreach (var line in Cells)
            {
                foreach (var cell in line)
                {
                    cell.ClearCreatures();
                }
            }
        }

        public List<CreatureInteraction> CellsOutcome()
        {
            List<CreatureInteraction> interactions = new List<CreatureInteraction>();

            foreach(var line in Cells)
            {
                foreach(var cell in line)
                {
                    if((cell.CreatureCount > 0 && cell.HasFood()) || (cell.CreatureCount > 1))
                    {
                        interactions.Add(new CreatureInteraction(cell.GetCreatures(), cell.HasFood()));
                        cell.ClearFood();
                    }
                }
            }

            return interactions;
        }
    }
}