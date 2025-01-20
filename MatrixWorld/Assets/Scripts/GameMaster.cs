using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GameMaster: MonoBehaviour
{
    [Header("Configuración de celdas")]

    [Tooltip("Celda de suelo.")]
    public GameObject floorTile;

    [Tooltip("Celda que representa al jugador.")]
    public GameObject playerTile;

    [Tooltip("Celda de muro.")]
    public GameObject wallTile;

    [Tooltip("Celda de escalera.")]
    public GameObject stairTile;

    [Tooltip("Celda de vacío.")]
    public GameObject voidTile;

    [Header("Tilemaps")]
    [Tooltip("Tilemap del mundo.")]
    public Tilemap tilemap;

    private PieceDict pieceDict;

    private GameObject[] entityInstances;

    private (int y, int x) playerPos;

    private void entityRendering() 
    {
        entityInstances = new GameObject[1];
        PieceData piece = pieceDict.getPiece(1); 
        GameObject tile = piece.tile;
        entityInstances[0] = Instantiate(tile, new Vector2(0, 0), Quaternion.identity);
        playerPos = (0, 0);
    }

    private TileBase[,] tiles;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        GameObject[] gameObjectsArray = new [] { floorTile, playerTile, wallTile, stairTile, voidTile };
        pieceDict = new PieceDict(gameObjectsArray);

        GetAllTilesAndPositions();
    }

    
    void GetAllTilesAndPositions()
    {
        BoundsInt bounds = tilemap.cellBounds;
        int width = bounds.xMax - bounds.xMin;
        int height = bounds.yMax - bounds.yMin;
        tiles = new TileBase[width, height];

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);
                TileBase tile = tilemap.GetTile(cellPosition);
                int adjustedX = x - bounds.xMin;
                int adjustedY = y - bounds.yMin;
                tiles[adjustedX, adjustedY] = tile;
                // if (tile != null)
                // {
                //     Debug.Log($"Tile encontrado en {cellPosition}: {tile.name}");
                // } else{
                //     Debug.Log($"Tile encontrado en {cellPosition}: vacio");;
                // }
            }
        }
        Debug.Log($"{tiles}");
    }
    
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        entityRendering();
        updateCamera();
    }

    void updateCamera()
    {
        if(playerPos.x != Camera.main.transform.position.x || playerPos.y != Camera.main.transform.position.y)
            Camera.main.transform.position = new Vector3(playerPos.x, -playerPos.y, Camera.main.transform.position.z);
    }

    (int y, int x) move((int y, int x) pos, (int y, int x) target)
    {
        if(tiles[target.y, target.x] == null) {
            return pos;
        }
        else if(tiles[target.y, target.x].name == "Floor") {
            entityInstances[0].transform.position = new Vector3(target.x, target.y, 0);
            return target;
        } 
        else if (tiles[target.y, target.x].name == "Wall"
            && (tiles[pos.y, pos.x].name == "Wall" || tiles[pos.y, pos.x].name == "Stair")){
            entityInstances[0].transform.position = new Vector3(target.x, target.y, 0);
            return target;
        }
        else if (tiles[target.y, target.x].name == "Stair"){
            entityInstances[0].transform.position = new Vector3(target.x, target.y, 0);
            return target;
        }
        else {
            return pos;
        }
        
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.a
    /// </summary>
    void Update()
    {
        BoundsInt bounds = tilemap.cellBounds;
        Debug.Log(playerPos+ "" + tilemap.cellBounds);

        if (Input.GetKeyDown(KeyCode.W)) 
        {
            (int y, int x) target = (playerPos.y - 1, playerPos.x);
            if (target.y + bounds.yMin >= bounds.yMin) {
                playerPos = move(playerPos, target);
            }
        }
        else if (Input.GetKeyDown(KeyCode.S)) 
        {
            (int y, int x) target = (playerPos.y + 1, playerPos.x);
            if (target.y + bounds.yMin < bounds.yMax) {
                playerPos = move(playerPos, target);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D)) 
        {
            (int y, int x) target = (playerPos.y, playerPos.x + 1);
            if (target.x + bounds.xMin < bounds.xMax) {
                playerPos = move(playerPos, target);
            }
        }
        else if (Input.GetKeyDown(KeyCode.A)) 
        {
            (int y, int x) target = (playerPos.y, playerPos.x - 1);
            if (target.x + bounds.xMin >= bounds.xMin) {
                playerPos = move(playerPos, target);
            }
        }
        updateCamera();
    }
}
