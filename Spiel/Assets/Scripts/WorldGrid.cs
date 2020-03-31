using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WorldGrid : MonoBehaviour
{
    [SerializeField]
    [TextArea(minLines:20, maxLines: 20)]

    GameObject[][] grid;


    int gridHeight = 35;
    int gridWidth = 40;
    int topMargin = 15;
    int bottomMargin = 9;
    int playerOffsetToLeft = 5;

    public static float cellSize = 1f;

    Vector2Int wallPos;
    Vector2Int wallDir;

    private GameObject lastWall;
    private GameObject firstWall;
    private Vector2Int lastWallGridPos;
    private GameObject currentTarget;
    private Wall animationTile;

    [SerializeField]
    private int playerStartNr = 10;

    [SerializeField]
    private int tilesToAnimateInFront = 10;

    private int noObstaclesInARow = 0;
    private int obstaclesInARow = 0;
    GameObject player;

    GameEventSystem gameEventSystem;

    void Start()
    {
        gameEventSystem = GameObject.FindGameObjectWithTag("GameEventSystem").GetComponent<GameEventSystem>();
        player = GameObject.FindGameObjectWithTag("Player");
        gameEventSystem.onPlayerTargetReached += OnPlayerTargetReached;
        gameEventSystem.onResetOrigin += ResetOrigin;
        gameEventSystem.onCrash += OnCrash;
        InitGrid();
        GenerateWall();
        StartGame();
    }


    private void InitGrid() {
        grid = new GameObject[gridWidth][];
        for (int i = 0; i < grid.Length; i++)
        {
            grid[i] = new GameObject[gridHeight];
        }

        wallPos = new Vector2Int(gridWidth - 1, 15);
        wallDir = Vector2Int.right;

        lastWall = null;
        firstWall = null;
    }

    void OnCrash()
    {
        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[i].Length; j++)
            {
                if(grid[i][j] != null) ObjectPool.current.RemoveGameobject(grid[i][j]);
                grid[i][j] = null;
            }
        }

        InitGrid();
        ResetOrigin();
        GenerateWall();
        StartGame();
    }

    void OnPlayerTargetReached() {
        currentTarget = currentTarget.GetComponent<Wall>().next;
        player.GetComponent<Movement>().setTarget(currentTarget.transform);
        
        if((player.transform.position.x - transform.position.x) > ((grid.Length/2) - playerOffsetToLeft))
        GenerateColumn();
        animationTile = animationTile.next.GetComponent<Wall>();
        animationTile.StartAppear();
    }

    private void StartGame()
    {
        currentTarget = firstWall;

        for(int i = 0; i <playerStartNr; i++)
        {
            currentTarget = currentTarget.GetComponent<Wall>().next;
        }

        player.transform.position = (currentTarget.transform.position + new Vector3(0, 4, 0));
        player.GetComponent<Movement>().setTarget(currentTarget.transform);
        Wall w = currentTarget.GetComponent<Wall>();
        for(int i = 0; i < tilesToAnimateInFront; i++)
        {
            w = w.next.GetComponent<Wall>();
        }
        animationTile = w;
        GameObject.FindGameObjectWithTag("GameEventSystem").GetComponent<GameEventSystem>().GridStart();
    }



    private void GenerateColumn()
    {
        

        bool inColumn = true;
        while (inColumn)
        {
            GameObject currentwall;
            if (GameStateManager.current.state != GameStateManager.PLAYSTATE)
            {
                currentwall = grid[wallPos.x][wallPos.y] = ObjectPool.current.CreateGameObject(ObjectPool.WALL, GetWorldPosition(wallPos), Quaternion.identity, transform);
            }
            else
            {
                if(Random.value < 0.3f && obstaclesInARow < 3 && noObstaclesInARow > 2)
                {
                    currentwall = grid[wallPos.x][wallPos.y] = ObjectPool.current.CreateGameObject(ObjectPool.OBSTACLE, GetWorldPosition(wallPos), Quaternion.identity, transform);
                    obstaclesInARow++;
                    noObstaclesInARow = 0;
                }
                else
                {
                    currentwall = grid[wallPos.x][wallPos.y] = ObjectPool.current.CreateGameObject(ObjectPool.WALL, GetWorldPosition(wallPos), Quaternion.identity, transform);
                    noObstaclesInARow++;
                    obstaclesInARow = 0;
                }
                
            }
            
       
            if(lastWall != null)
            {
                lastWall.GetComponent<Wall>().next = currentwall;
            }
            else
            {
                firstWall = currentwall;
            }
            lastWall = currentwall;




            if(Random.value <= 0.3f) MakeTurn();
            int newY = (wallPos + wallDir).y;
            if (newY > (grid[0].Length - topMargin) || newY < bottomMargin) MakeTurn();
            if(wallDir == Vector2Int.right)
            {
                inColumn = false;
            }
            else
            {
                wallPos += wallDir;
            }
        }

        for (int i = 0; i < grid[wallPos.x].Length; i++)
        {
            if (grid[wallPos.x][i] == null)
            {
                grid[wallPos.x][i] = ObjectPool.current.CreateGameObject(ObjectPool.GROUND, GetWorldPosition(wallPos.x, i), Quaternion.identity, transform);  //GameObject.Instantiate(groundPrefab, GetWorldPosition(wallPos.x, i), Quaternion.identity, transform);
            }
        }

        ShiftGridToLeft();

    }

    private void ShiftGridToLeft()
    {
        //shift grid to the right by 1
        transform.Translate(Vector3.right.normalized);
        

        GameObject[] toDelete = new GameObject[grid[0].Length];
        //Remember left (first) column for deletion
        for (int i = 0; i < toDelete.Length; i++)
        {
            toDelete[i] = grid[0][i];
        }
       
        
        //shift the grid[][] to left
        for(int i = 0; i < grid.Length - 1; i++)
        {
            for(int j = 0; j < grid[i].Length; j++)
            {
                grid[i][j] = grid[i + 1][j];
            }
            
        }

        //Remove objectreferences from grid;
        for (int i = 0; i < grid[grid.Length - 1].Length; i++)
        {
            grid[grid.Length - 1][i] = null;
        }


        //delete first column column
        for (int i = 0; i < toDelete.Length; i++)
        {
            if (toDelete[i] != null) ObjectPool.current.RemoveGameobject(toDelete[i]);  //GameObject.Destroy(toDelete[i]);
        }


        //Move all Gridobjects to the left;
        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[i].Length; j++)
            {
                if (grid[i][j] != null) grid[i][j].transform.Translate(Vector3.left.normalized);
            }
        }


    }

    private void MakeTurn()
    {

        if (wallDir == Vector2Int.right)
        {
            wallDir = Random.value > 0.5f ? Vector2Int.up : Vector2Int.down;
        }
        else
        {
            wallDir = Vector2Int.right;
        }
    }

    private void GenerateWall()
    {
        for(int i = 0; i < grid.Length - 1; i++)
        {
            GenerateColumn();
        }

        GameObject w = firstWall;

        while(w != null)
        {
            Wall ws = w.GetComponent<Wall>();
            ws.StartAppear();
            w = ws.next;
        }

       

    }



    private Vector3 GetWorldPosition(int x, int y)
    {
        return transform.position + new Vector3(x, 0, y) * cellSize;
    }

    private Vector3 GetWorldPosition(Vector2Int gridPos)
    {
        return GetWorldPosition(gridPos.x, gridPos.y);
    }

    private void ResetOrigin()
    {
        //float playerPosX = Player.current.transform.position.x;
        //transform.Translate(Vector3.left * playerPosX);
        //Player.current.transform.Translate(Vector3.left * playerPosX);

        transform.position = Vector3.zero;

    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[i].Length; j++)
            {
                Debug.DrawLine(i * cellSize * Vector3.right + Vector3.forward * j + transform.position, (i + 1) * cellSize * Vector3.right + Vector3.forward * j +transform.position);
                Debug.DrawLine(i * cellSize * Vector3.right + Vector3.forward * j + transform.position, (i) * cellSize * Vector3.right + Vector3.forward * (j + 1) + transform.position);
            }
        }

        if (Input.GetKeyDown("space"))
        {
            OnCrash();
        }

    }
}
