using Assets.Scripts.Logic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FieldController : MonoBehaviour
{
    public GroundChessBoard bedrockVoxel;

    public GameObject BrickWall;
    public GameObject WaterBlock;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject mainCamera;
    public GameObject FlagSphere;
    

    public float playerSpeed = 3;
    public float enemySpeed = 2;
    public float attackTime = 15;
    public float spawnTime = 4;
    public bool isWall;




    private Cell[,] cells;
    private int width;
    private int height;
    private Player player;
    private EnemyAI enemy_;
    private int a;
    private int b;
    private int stopp;

    private string[] map = new[] {
        ".P.BFB.WB.",
        "...BBB....",
        "..#..B..W.",
        ".....B#.W.",
        "WW.B.BW.#.",
        "...B.B#...",
        "...B.B...#",
        ".......#..",
        ".....B....",
        ".E....B...",
    };

    void Start()
    {
        height = map.Length;
        width = map[0].Length;

        cells = new Cell[width + 2, height + 2];

        for (int i = 0; i < height; i++)
        {
            if (map[i].Length != width)
            {
                throw new System.Exception("Invalid map");
            }
            for (int j = 0; j < width; j++)
            {
                //cells[j + 1, i + 1] = new Cell(map[i][j] == '#' ? CellSpace.Bedrock : CellSpace.Empty);
                //cells[j + 1, i + 1] = new Cell(map[i][j] == 'B' ? CellSpace.Brick : CellSpace.Empty);

                if (map[i][j] == '#')
                {
                    cells[j + 1, i + 1] = new Cell(CellSpace.Bedrock);

                }
                else if (map[i][j] == 'B')
                {
                    cells[j + 1, i + 1] = new Cell(CellSpace.Brick);

                }
                else if (map[i][j] == 'W')
                {
                    cells[j + 1, i + 1] = new Cell(CellSpace.Water);

                }
                else if (map[i][j] == 'F')
                {
                    cells[j + 1, i + 1] = new Cell(CellSpace.Flag);

                }
                else
                {
                    cells[j + 1, i + 1] = new Cell(CellSpace.Empty);
                }

                if (map[i][j] == 'P')
                {
                    var playerGO = Instantiate(playerPrefab, new Vector3(j + 1, 1, i + 1), Quaternion.identity, transform);
                    player = playerGO.GetComponent<Player>();
                    player.Initialize(playerSpeed, cells);
                    cells[j + 1, i + 1].Occupy(player);
                }
                if (map[i][j] == 'E')
                {
                    var enemyGO = Instantiate(enemyPrefab, new Vector3(j + 1, 1, i + 1), Quaternion.identity, transform);
                    a = j;
                    b = i;
                    enemy_ = enemyGO.GetComponent<EnemyAI>();
                    enemy_.Initialize(enemySpeed, cells);
                    cells[j + 1, i + 1].Occupy(enemy_);

                }
                
            }
        }

        for (int i = 0; i < width + 2; i++)
        {
            cells[i, 0] = new Cell(CellSpace.Bedrock);
            cells[i, height + 1] = new Cell(CellSpace.Bedrock);
        }

        for (int i = 0; i < height + 2; i++)
        {
            cells[0, i] = new Cell(CellSpace.Bedrock);
            cells[width + 1, i] = new Cell(CellSpace.Bedrock);
        }

        for (var x = 0; x < width + 2; x++)
        {
            for (var y = 0; y < height + 2; y++)
            {
                var c = Instantiate(bedrockVoxel, new Vector3(x, 0, y), Quaternion.identity, transform);
                c.SetColor((x + y) % 2 == 0);

                if (cells[x, y].Space == CellSpace.Bedrock)
				{
                    Instantiate(bedrockVoxel, new Vector3(x, 1, y), Quaternion.identity, transform);
                }

                if (cells[x, y].Space == CellSpace.Brick)
                {
                    Instantiate(BrickWall, new Vector3(x, 0.4f, y), Quaternion.identity, transform);
                    
                }
                if (cells[x, y].Space == CellSpace.Water)
                {
                    Instantiate(WaterBlock, new Vector3(x, 0.01f, y), Quaternion.identity, transform);
                }
                if (cells[x, y].Space == CellSpace.Flag)
                {
                    Instantiate(FlagSphere, new Vector3(x, 1, y), Quaternion.identity, transform);
                }
            }
        }

        mainCamera.transform.position = new Vector3((width + 2) / 2, 11,(height + 2) / 6);
        mainCamera.transform.eulerAngles = new Vector3(75, 0, 0);
        stopp = 0;
    }
    public void Reload()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    // Update is called once per frame
    void Update()
    {
        
        if (spawnTime > 0 && stopp < 4)
        {
            spawnTime -= Time.deltaTime;
            
        }
        else if (stopp < 4)
        {
           

            var enemyGO = Instantiate(enemyPrefab, new Vector3(a + 1, 1, b + 1), Quaternion.identity, transform);
            enemy_ = enemyGO.GetComponent<EnemyAI>();
            enemy_.Initialize(enemySpeed, cells);
            cells[a + 1, b + 1].Occupy(enemy_);


            spawnTime = (float)(4);

            stopp = stopp + 1;
        }


        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(player.TryMove(Vector2Int.right));
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine(player.TryMove(Vector2Int.left));
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            StartCoroutine(player.TryMove(Vector2Int.up));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(player.TryMove(Vector2Int.down));
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.Fire();
        }

        if (Input.GetKeyDown(KeyCode.Escape))  // если нажата клавиша Esc (Escape)
        {
            Application.Quit();    // закрыть приложение
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }



        for (var x = 0; x < cells.GetLength(0); x++)
        {
            for (var y = 0; y < cells.GetLength(1); y++)
            {
                if (cells[x, y].Occupant is EnemyAI enemy)
                {
                    enemy.StartCoroutine(enemy.Think());
                }
            }
        }
        

        




    }

}
