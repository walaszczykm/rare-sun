using System;
using System.Collections.Generic;

namespace mazes
{
    static class AldousBroder
    {
        public static Grid Using(Grid grid)
        {
            Random random = new Random();
            Cell cell = grid.RandomCell;
            int unvisited = grid.Size - 1;

            while(unvisited > 0)
            {
                List<Cell> neighbors = cell.Neighbors;
                Cell neighbor = neighbors[random.Next(neighbors.Count)];
                if(neighbor.Links.Count == 0)
                {
                    cell.Link(neighbor);
                    --unvisited;
                }
                cell = neighbor;
            }

            return grid;
        }
    }
}
