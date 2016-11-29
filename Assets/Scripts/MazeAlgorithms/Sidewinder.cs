using System;
using System.Collections.Generic;

namespace MazeAlgorithms
{
    class Sidewinder : MazeAlgorithm
    {
        public override Grid Using(Grid grid)
        {
            Random random = new Random();

            grid.ForeachRow(row =>
            {
                List<Cell> run = new List<Cell>();
                foreach(Cell cell in row)
                {
                    run.Add(cell);

                    bool atEasternBoundary = (cell.east == null);
                    bool atNorthenBoundary = (cell.north == null);

                    bool shouldCloseOut = atEasternBoundary ||
                    (!atNorthenBoundary && random.Next(2) == 0);

                    if(shouldCloseOut)
                    {
                        int randomIndex = random.Next(run.Count);
                        Cell randomRunMember = run.Find(member =>
                        run.IndexOf(member) == randomIndex);

                        if(randomRunMember.north != null)
                        {
                            randomRunMember.Link(randomRunMember.north);
                        }

                        run.Clear();
                    }
                    else
                    {
                        cell.Link(cell.east);
                    }
                }
            });

            return grid;
        }
    }
}
