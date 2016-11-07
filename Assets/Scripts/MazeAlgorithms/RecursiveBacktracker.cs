using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeAlgorithms
{
    class RecursiveBacktracker : MazeAlgorithm
    {
        public override Grid Using(Grid grid)
        {
            Random random = new Random();
            Stack<Cell> stack = new Stack<Cell>();
            stack.Push(grid.RandomCell);

            while(stack.Count > 0)
            {
                Cell current = stack.Peek();

                List<Cell> notVisitedNeighbors = current.Neighbors.Where(
                    cell => cell.Links.Count == 0).ToList();

                if(notVisitedNeighbors.Count == 0)
                {
                    stack.Pop();
                }
                else
                {
                    int randomIndex = random.Next(notVisitedNeighbors.Count);
                    Cell neighbor = notVisitedNeighbors.Find(
                        cell => notVisitedNeighbors.IndexOf(cell) == randomIndex);

                    current.Link(neighbor);
                    stack.Push(neighbor);
                }
            }
            return grid;
        }
    }
}
