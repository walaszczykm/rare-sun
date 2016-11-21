using System.Collections.Generic;

namespace MazeAlgorithms
{   
    public class Cell
    {
        public Cell north, west, east, south;
        public float x, z;
        private int row, column;
        private List<Cell> links;
        private System.Random random = new System.Random();

        public int Row
        {
            get
            {
                return row;
            }
        }

        public int Column
        {
            get
            {
                return column;
            }
        }

        public List<Cell> Links
        {
            get
            {
                return links;
            }
        }

        public Cell RandomLink
        {
            get
            {
                if (links.Count > 0)
                {
                    int idx = random.Next(0, links.Count);
                    return links[idx];
                }
                else
                {
                    return null;
                }
            }
        }

        public List<Cell> Neighbors
        {
            get
            {
                List<Cell> neighborsList = new List<Cell>();
                if(north != null) neighborsList.Add(north);
                if(west != null) neighborsList.Add(west);
                if(east != null) neighborsList.Add(east);
                if(south != null) neighborsList.Add(south);
                return neighborsList;
            }
        }

        public Cell(int row, int column)
        {
            this.row = row;
            this.column = column;
            this.links = new List<Cell>();
        }

        public void Link(Cell cell, bool bidi=true)
        {
            links.Add(cell);
            if(bidi) cell.Link(this, false);
        }

        public void Unlink(Cell cell, bool bidi=true)
        {
            links.Remove(cell);
            if(bidi) cell.Unlink(this, false);
        }
        
        public bool IsLinked(Cell cell)
        {
            if (cell != null)
            {
                return links.Contains(cell);
            }
            else
            {
                return false;
            }
        }
    }
}
