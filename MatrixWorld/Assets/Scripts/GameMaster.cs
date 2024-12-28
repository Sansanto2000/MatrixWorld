using UnityEngine;
using System.Collections.Generic;

public class GameMaster: MonoBehaviour
{
    [Header("Configuración de celdas")]
    [Tooltip("Celda de vacío.")]
    public GameObject voidTile;
    [Tooltip("Celda que representa al jugador.")]
    public GameObject playerTile;

    private PieceDict pieceDict;

    private GameObject[,] entityInstances;
    private int[,] entityLayer = {
        {0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0},
        {0,0,0,1,0,0,0},
        {0,0,0,0,0,0,0},
    };

    private int[,] world = {
        {0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0},
    };

    private (int y, int x) playerPos;


    private void worldRendering() 
    {
        for (int i = 0; i < world.GetLength(0); i++)
        {
            for (int j = 0; j < world.GetLength(1); j++)
            {
                GameObject tile;
                PieceData piece = pieceDict.getPiece(world[i,j]);
                tile = piece.tile;
                Instantiate(tile, new Vector2(j, -i), Quaternion.identity);
            }
        }
    }

    private void entityRendering() 
    {
        int rows = entityLayer.GetLength(0);
        int cols = entityLayer.GetLength(1);
        entityInstances = new GameObject[rows, cols];
        GameObject instance;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                GameObject tile;
                PieceData piece = pieceDict.getPiece(entityLayer[i,j]); 
                tile = piece.tile;
                if(piece.name == "Player") {
                    playerPos = (i, j);
                    instance = Instantiate(tile, new Vector2(j, -i), Quaternion.identity);
                } else {
                    instance = null;
                }
                entityInstances[i,j] = instance;
            }
        }
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        GameObject[] gameObjectsArray = new [] { voidTile, playerTile };
        pieceDict = new PieceDict(gameObjectsArray);
    }
    
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        worldRendering();
        entityRendering();
    }

    (int y, int x) move(int[,] world, (int y, int x) pos, (int y, int x) target)
    {
        if(world[target.y, target.x] == PieceDict.Void.code) {
            // Movimiento
            entityInstances[playerPos.y, playerPos.x].transform.position = new Vector3(target.x, -target.y, 0);
            // Referencias
            entityInstances[target.y, target.x] = entityInstances[playerPos.y, playerPos.x];
            entityLayer[target.y, target.x] = PieceDict.Player.code;
            entityInstances[playerPos.y, playerPos.x] = null;
            entityLayer[playerPos.y, playerPos.x] = PieceDict.Void.code;
            
            return target;
        } else {
            return pos;
        }
        
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) 
        {
            (int y, int x) target = (playerPos.y - 1, playerPos.x);
            if (target.y >= 0) {
                playerPos = move(world, playerPos, target);
            }
        }
        else if (Input.GetKeyDown(KeyCode.S)) 
        {
            (int y, int x) target = (playerPos.y + 1, playerPos.x);
            if (target.y < world.GetLength(0)) {
                playerPos = move(world, playerPos, target);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D)) 
        {
            (int y, int x) target = (playerPos.y, playerPos.x + 1);
            if (target.x < world.GetLength(1)) {
                playerPos = move(world, playerPos, target);
            }
        }
        else if (Input.GetKeyDown(KeyCode.A)) 
        {
            (int y, int x) target = (playerPos.y, playerPos.x - 1);
            if (target.x >= 0) {
                playerPos = move(world, playerPos, target);
            }
        }
    }
}
