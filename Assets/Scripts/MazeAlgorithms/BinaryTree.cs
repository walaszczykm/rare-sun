using System;
using System.Threading;
using System.Collections.Generic;

namespace MazeAlgorithms
{
    static class BinaryTree
    {
        public static Grid Using(Grid grid)
        {
            Random random = new Random();
            grid.EachCell(cell =>
            {
                List<Cell> neighbors = new List<Cell>();

                if(cell.north != null) neighbors.Add(cell.north);
                if(cell.east != null) neighbors.Add(cell.east);

                if (neighbors.Count > 0)
                {
                    int index = random.Next(neighbors.Count);
                    Cell neighbor = neighbors[index];

                    if (neighbor != null) cell.Link(neighbor);
                }
            });

            return grid;
        }
    }
}
