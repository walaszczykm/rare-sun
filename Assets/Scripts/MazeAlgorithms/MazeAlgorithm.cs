using System.Collections.Generic;

namespace MazeAlgorithms
{
    public class MazeAlgorithm
    {
        public static readonly Dictionary<string, MazeAlgorithm> Algorithms = 
        new Dictionary<string, MazeAlgorithm>()
        {
            { typeof(BinaryTree).Name, new BinaryTree()},
            { typeof(AldousBroder).Name, new AldousBroder()}
        };

        public virtual Grid Using(Grid grid)
        {
            return grid;
        }
    }
}
