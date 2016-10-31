using UnityEngine;
using MazeAlgorithms;
using System.IO;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private int rows, columns;

    private const float CELL_SIZE = 5.0f;
    private GameObject cellPrefab;
    private Grid grid;

    private void Awake()
    {
        cellPrefab = Resources.Load<GameObject>(Paths.Prefabs.CELL);
        grid = AldousBroder.Using(new Grid(rows, columns));
        File.WriteAllText("grid.txt", grid.ToString());
    }

    private void Start()
    {
        InstCells();
    }

    private void InstCells()
    {
        Transform mazeParent = new GameObject("Maze").transform;
        //iterate through each cell in grid
        grid.EachCell(cell =>
        {
            //instantiate new cell gmae object based on cellPrefab
            Transform cellTrans = Instantiate(cellPrefab).transform;
            cellTrans.SetParent(mazeParent);
            CellComponent cellComp = cellTrans.GetComponent<CellComponent>();
            if(cellComp != null)
            {
                //Setting position of cell game object
                Vector3 cellPos = cellTrans.position;
                cellPos.x = cell.Column * CELL_SIZE;
                cellPos.z = cell.Row * CELL_SIZE * -1;
                cellTrans.position = cellPos;

                //call SetPassages to destroy not needed eits in cell game object
                cellComp.SetPassages(cell);
            }
            else
            {
                Debug.LogError("LevelGenerator.InstCell - CellComponent is missing.");
            }
        });
    }
}
