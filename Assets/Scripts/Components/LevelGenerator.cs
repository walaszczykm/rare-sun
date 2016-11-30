using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MazeAlgorithms;
using System.Linq;

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

    #region Consts
    public const float CELL_SIZE = 5.0f;
    public const string MAZE_ALGORITHM_PREFS_KEY = "ALGORITHM";
    public const string MAZE_ROWS_PREFS_KEY = "ROWS";
    public const string MAZE_COLUMNS_PREFS_KEY = "COLUMNS";
    #endregion

    private int rows, columns;
    private Grid grid;
    private Transform mazeParent = null;
    private MazeAlgorithm algorithm;
    private Dictionary<string, GameObject> prefabsDictionary;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        PrewarmPrefabs();
        rows = 2;
        columns = 2;
    }

    private void Start()
    {
        CheckPrefsKeys();
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

        grid = algorithm.Using(new Grid(rows, columns));
        mazeParent = new GameObject("Maze").transform;

        grid.ForeachCell(cell =>
        {
            Vector3 cellPos = InstCell(mazeParent, cell);

            if(cell.Column == grid.Columns-1 && cell.Row == grid.Rows-1)
            {
                GameObject exit = InstExit();
                exit.transform.position = new Vector3(cellPos.x, 0.9f, cellPos.z);
                exit.transform.SetParent(mazeParent);
            }
            else if(!(cell.Row == 0 && cell.Column == 0))
            {
                GameObject pickup = InstRandomPickup();
                pickup.transform.position = new Vector3(cellPos.x, 0.5f, cellPos.z);
                pickup.transform.SetParent(mazeParent);
                pickup.GetComponent<Pickup>().SetupPickup();
                
                GameObject enemy = InstRandomEnemy();
                if (enemy != null)
                {
                    enemy.transform.position = new Vector3(cellPos.x, 0.5f, cellPos.z);
                    enemy.transform.SetParent(mazeParent);
                    enemy.GetComponent<Enemy>().Init(cell);
                }
            }
        });
    }

    private void PrewarmPrefabs()
    {
        prefabsDictionary = new Dictionary<string, GameObject>()
        {
            {
                Paths.Prefabs.FIRST_AID_KIT,
                Resources.Load<GameObject>(Paths.Prefabs.FIRST_AID_KIT)
            },
            {
                Paths.Prefabs.CELL,
                Resources.Load<GameObject>(Paths.Prefabs.CELL)
            },
            {
                Paths.Prefabs.COIN,
                Resources.Load<GameObject>(Paths.Prefabs.COIN)
            },
            {
                Paths.Prefabs.ENEMY,
                Resources.Load<GameObject>(Paths.Prefabs.ENEMY)
            },
            {
                Paths.Prefabs.EXIT,
                Resources.Load<GameObject>(Paths.Prefabs.EXIT)
            },
            {
                Paths.Prefabs.PLAYER,
                Resources.Load<GameObject>(Paths.Prefabs.PLAYER)
            },
            {
                Paths.Prefabs.Weapons.PISTOL,
                Resources.Load<GameObject>(Paths.Prefabs.Weapons.PISTOL)
            },
            {
                Paths.Prefabs.Weapons.SMG,
                Resources.Load<GameObject>(Paths.Prefabs.Weapons.SMG)
            }
        };
    }

    private Vector3 InstCell(Transform parent, Cell cell)
    {
        Transform cellTrans = Instantiate(
            prefabsDictionary[Paths.Prefabs.CELL]).transform;
        cellTrans.SetParent(parent);
        CellComponent cellComp = cellTrans.GetComponent<CellComponent>();

        Vector3 cellPos = cellTrans.position;
        cellPos.x = cell.Column * CELL_SIZE;
        cellPos.z = cell.Row * CELL_SIZE * -1;

        cell.x = cellPos.x;
        cell.z = cellPos.z;

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
                return Instantiate(prefabsDictionary[Paths.Prefabs.FIRST_AID_KIT]);
            }
            else
            {
                r = Random.Range(0, Weapon.ModelsNumber);
                string weaponName = ((Weapon.Model)r).ToString();
                return Instantiate(prefabsDictionary[Paths.Prefabs.WEAPONS + weaponName]);
            }
        }
        else
        {
            return Instantiate(prefabsDictionary[Paths.Prefabs.COIN]);
        }
    }

    private GameObject InstRandomEnemy()
    {
        int r = Random.Range(0, 7);
        if(r == 1)
        {
            return Instantiate(prefabsDictionary[Paths.Prefabs.ENEMY]);
        }
        return null;
    }

    private void InstPlayer()
    {
        GameObject playerGO = Instantiate(prefabsDictionary[Paths.Prefabs.PLAYER]);
        playerGO.name = "Player";
        Player player = playerGO.GetComponent<Player>();
        player.ResetPlayer();
    }

    private GameObject InstExit()
    {
        return Instantiate(prefabsDictionary[Paths.Prefabs.EXIT]);
    }

    private void CheckPrefsKeys()
    {
        if(PlayerPrefs.HasKey(MAZE_ROWS_PREFS_KEY))
        {
            int rows = PlayerPrefs.GetInt(MAZE_ROWS_PREFS_KEY);
            if(rows >= 2)
            {
                this.rows = rows;
            }
        }
        if(PlayerPrefs.HasKey(MAZE_COLUMNS_PREFS_KEY))
        {
            int columns = PlayerPrefs.GetInt(MAZE_COLUMNS_PREFS_KEY);
            if(columns >= 2)
            {
                this.columns = columns;
            }
        }
        if(PlayerPrefs.HasKey(MAZE_ALGORITHM_PREFS_KEY))
        {
            algorithm = MazeAlgorithm.Algorithms[PlayerPrefs.GetString(MAZE_ALGORITHM_PREFS_KEY)];
        }
        else
        {
            algorithm = MazeAlgorithm.Algorithms.Values.ToArray()[0];
        }
    }
}
