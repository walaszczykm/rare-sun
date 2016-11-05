using UnityEngine;
using UnityEngine.SceneManagement;
using MazeAlgorithms;
using System.IO;

public class LevelGenerator : MonoBehaviour
{
    private static LevelGenerator instance;
    public static LevelGenerator Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField]
    private int rows, columns;

    private const float CELL_SIZE = 5.0f;
    private GameObject cellPrefab;
    private Grid grid;
    private Transform mazeParent = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        cellPrefab = Resources.Load<GameObject>(Paths.Prefabs.CELL);
    }

    private void Start()
    {
        GenerateLevel();
        InstPlayer();
    }

    public void OnRestart()
    {
        SceneManager.LoadScene("Game");
    }

    public void DestroyLevel()
    {
        Destroy(mazeParent.gameObject);
    }

    public void GenerateLevel()
    {
        if(mazeParent != null)
        {
            ++rows;
            ++columns;
            DestroyLevel();
        }

        grid = AldousBroder.Using(new Grid(rows, columns));
        mazeParent = new GameObject("Maze").transform;

        grid.EachCell(cell =>
        {
            Vector3 cellPos = InstCell(mazeParent, cell);

            if(!(cell.Row == 0 && cell.Column == 0))
            {
                //position ranodm pickup and activate it
                GameObject pickup = InstRandomPickup();
                pickup.transform.position = new Vector3(cellPos.x, 0.5f, cellPos.z);
                pickup.transform.SetParent(mazeParent);
                pickup.GetComponent<Pickup>().SetupPickup();

                //position random enemies
                GameObject enemy = InstRandomEnemy();
                if (enemy != null)
                {
                    enemy.transform.position = new Vector3(cellPos.x, 0.5f, cellPos.z);
                    enemy.transform.SetParent(mazeParent);
                }
            }

            if(cell.Column == grid.Columns-1 && cell.Row == grid.Rows-1)
            {
                GameObject exit = InstExit();
                exit.transform.position = new Vector3(cellPos.x, 0.9f, cellPos.z);
                exit.transform.SetParent(mazeParent);
            }
        });
    }

    private Vector3 InstCell(Transform parent, Cell cell)
    {
        Transform cellTrans = Instantiate(cellPrefab).transform;
        cellTrans.SetParent(parent);
        CellComponent cellComp = cellTrans.GetComponent<CellComponent>();

        Vector3 cellPos = cellTrans.position;
        cellPos.x = cell.Column * CELL_SIZE;
        cellPos.z = cell.Row * CELL_SIZE * -1;
        cellTrans.position = cellPos;

        cellComp.SetPassages(cell);

        return cellPos;
    }

    private GameObject InstRandomPickup()
    {
        int r = Random.Range(0, 5);
        if(r == 1)
        {
            r = Random.Range(0, 2);
            if(r == 1)
            {
                //spawn FirstAidKit
                return Instantiate(
                    Resources.Load<GameObject>(Paths.Prefabs.FIRST_AID_KIT));
            }
            else
            {
                //spawn Weapon
                r = Random.Range(0, Weapon.ModelsNumber);
                string weaponName = ((Weapon.Model)r).ToString();
                return Instantiate(
                    Resources.Load<GameObject>(Paths.Prefabs.WEAPONS + weaponName));
            }
        }
        else
        {
            return Instantiate(Resources.Load<GameObject>(Paths.Prefabs.COIN));
        }
    }

    private GameObject InstRandomEnemy()
    {
        int r = Random.Range(0, 7);
        if(r == 1)
        {
            return Instantiate(Resources.Load<GameObject>(Paths.Prefabs.ENEMY));
        }
        return null;
    }

    private void InstPlayer()
    {
        GameObject playerGO = Instantiate(Resources.Load<GameObject>(Paths.Prefabs.PLAYER));
        playerGO.name = "Player";
        Player player = playerGO.GetComponent<Player>();
        player.ResetPlayer();
    }

    private GameObject InstExit()
    {
        return Instantiate(Resources.Load<GameObject>(Paths.Prefabs.EXIT));
    }
}
