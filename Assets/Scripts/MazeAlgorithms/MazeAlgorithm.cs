using System.Collections.Generic;

namespace MazeAlgorithms
{
    public abstract class MazeAlgorithm
    {
        public static readonly Dictionary<string, MazeAlgorithm> Algorithms = 
        new Dictionary<string, MazeAlgorithm>()
        {
            { typeof(BinaryTree).Name, new BinaryTree()},
            { typeof(AldousBroder).Name, new AldousBroder()},
            { typeof(RecursiveBacktracker).Name, new RecursiveBacktracker()}
        };

        public abstract Grid Using(Grid grid);
    }
}
