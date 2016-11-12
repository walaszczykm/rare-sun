using UnityEngine;
using MazeAlgorithms;

public class CellComponent : MonoBehaviour
{
    [SerializeField]
    private GameObject northExit;
    [SerializeField]
    private GameObject southExit;
    [SerializeField]
    private GameObject westExit;
    [SerializeField]
    private GameObject eastExit;
    private Cell cell;
    public Cell Cell
    {
        get
        {
            return cell;
        }
    }

    public void SetPassages(Cell cell)
    {
        this.cell = cell;
        if(cell.IsLinked(cell.north)) Destroy(northExit);
        if(cell.IsLinked(cell.south)) Destroy(southExit);
        if(cell.IsLinked(cell.east)) Destroy(eastExit);
        if(cell.IsLinked(cell.west)) Destroy(westExit);
    }    
}
