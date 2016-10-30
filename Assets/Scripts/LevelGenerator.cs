using UnityEngine;
using MazeAlgorithms;
using System.IO;

public class LevelGenerator : MonoBehaviour
{
    private const int CELL_SIZE = 5;
    private GameObject cellPrefab;
    private Grid grid;

    void Awake()
    {
        cellPrefab = Resources.Load<GameObject>(Paths.Prefabs.CELL);
        grid = BinaryTree.Using(new Grid(4, 4));
        File.WriteAllText("grid.txt", grid.ToString());
    }

    void Start()
    {
        InstCells();
    }

    private void InstCells()
    {
        //iterate through each cell in grid
        grid.EachCell(cell =>
        {
            //instantiate new cell gmae object based on cellPrefab
            Transform cellTrans = Instantiate(cellPrefab).transform;
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
                Debug.LogError("InstCell - CellComponent is missing.");
            }
        });
    }
}
