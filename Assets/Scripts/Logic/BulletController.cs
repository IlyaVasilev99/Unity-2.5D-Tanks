using Assets.Scripts.Logic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class BulletController : MonoBehaviour
{
    private Vector2Int direction;
    public GameOverMenu GameOverMenu;
    public int MaxP;

    public bool defeat = false;
    public bool victory = false;

    public float speed = 10;
    private Cell[,] cells;
    public bool isWall;
    //public destroybrick script;
    //private float bulletTime = 0.05f;
    public GameObject Explosion;

    public void Initialize(Cell[,] cells)
    {
        this.cells = cells;
    }

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame

    void Update()
    {
        
        transform.position += new Vector3(direction.x, 0, direction.y) * speed * Time.deltaTime;

        var ourCell = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
        if (cells[ourCell.x, ourCell.y].Space == CellSpace.Bedrock)
        {
            explode();
        }

        if (cells[ourCell.x, ourCell.y].Space == CellSpace.Brick)
        {

            explode();

            cells[ourCell.x, ourCell.y] = new Cell(CellSpace.Empty);


        }
        if (cells[ourCell.x, ourCell.y].Space == CellSpace.Flag)
        {
            Debug.Log("Game Over! Your base was destroyed!");
            explode();
            GameOverLose();


            cells[ourCell.x, ourCell.y] = new Cell(CellSpace.Empty);
        }




        if (cells[ourCell.x, ourCell.y].Occupant != null)
        {
            var enemy = cells[ourCell.x, ourCell.y].Occupant.GetComponent<EnemyAI>();


            if (enemy != null)
            {
                enemy.Die();
                isWall = true;
                Debug.Log("Enemy tank was destroyed! Well done!");
                explode();
            }
        }


    }

    public void Fire(Vector2Int direction)
    {
        this.direction = direction;
    }

    void explode()
    {
        Debug.Log("Boom!");
        Instantiate(Explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    void GameOverLose()
    {
        GameOverMenu.Setup();
    }
}
