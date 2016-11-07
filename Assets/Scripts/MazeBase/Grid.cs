using System;

namespace MazeAlgorithms
{
    public class Grid
    {
        int rows, columns;
        Cell[][] grid;

        public int Rows
        {
            get
            {
                return rows;
            }
        }

        public int Columns
        {
            get
            {
                return columns;
            }
        }


        public int Size
        {
            get
            {
                return rows * columns;
            }
        }

        public Cell RandomCell
        {
            get
            {
                Random random = new Random();
                int row = random.Next(rows);
                int col = random.Next(columns);
                return this[row, col];
            }
        }

        public Cell this[int row, int column]
        {
            get
            {
                if (row < 0 || row > (rows - 1)) return null;
                if (column < 0 || column > (columns - 1)) return null;
                return grid[row][column];
            }
        }

        public Grid(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            grid = PrepareGrid();
            ConfigureCells();
        }

        public void ForeachCell(Action<Cell> action)
        {
            if (action != null)
            {
                for (int row = 0; row < rows; row++)
                {
                    for (int col = 0; col < columns; col++)
                    {
                        action(grid[row][col]);
                    }
                }
            }
        }

        public void ForeachRow(Action<Cell[]> action)
        {
            if (action != null)
            {
                for (int row = 0; row < rows; row++)
                {
                    action(grid[row]);
                }
            }
        }

        public override string ToString()
        {
            string output = "+";
            for(int i=0; i<columns; i++)
            {
                output += "---+";
            }
            output += "\n";

            ForeachRow(row =>
            {
                string top = "|";
                string bottom = "+";
                foreach(Cell cell in row)
                {
                    string body = "   ";
                    string eastBoundary = cell.IsLinked(cell.east) ? " " : "|";
                    top += body + eastBoundary;

                    string corner = "+";
                    string southBoundary = cell.IsLinked(cell.south) ? "   " : "---";
                    bottom += southBoundary + corner;
                }
                output += top + "\n";
                output += bottom + "\n";
            });
            return output;
        }

        private Cell[][] PrepareGrid()
        {
            Cell[][] newGrid = new Cell[rows][];
            for (int row = 0; row < rows; row++)
            {
                newGrid[row] = new Cell[columns];
                for (int col = 0; col < columns; col++)
                {
                    newGrid[row][col] = new Cell(row, col);
                }
            }
            return newGrid;
        }

        private void ConfigureCells()
        {
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    Cell cell = grid[row][col];
                    cell.north = this[row - 1, col];
                    cell.south = this[row + 1, col];
                    cell.west = this[row, col - 1];
                    cell.east = this[row, col + 1];
                }
            }
        }


    }
}
