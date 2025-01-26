using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GameMaster: MonoBehaviour
{
    [Header("Configuración de celdas")]

    [Tooltip("Celda que representa al jugador.")]
    public GameObject playerTile;

    [Header("Tilemaps")]
    [Tooltip("Tilemap del mundo.")]
    public Tilemap tilemap;

    [Header("Camera")]
    [Tooltip("Quiet")]
    public bool cameraQuiet = false;

    private PieceDict pieceDict;

    private GameObject player;

    private TileBase[,] tiles;

    private void entityRendering() 
    {
        Vector3Int originCell = tilemap.WorldToCell(new Vector3(0, 0, 0));
        Vector3 originPosFix = tilemap.GetCellCenterWorld(originCell);
        player = Instantiate(playerTile, originPosFix, Quaternion.identity);
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
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
                //     Debug.Log($"{cellPosition}: {tile.name}");
                // } else{
                //     Debug.Log($"{cellPosition}: vacio");;
                // }
            }
        }
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
        if(cameraQuiet){
            return;
        }
        Vector3  playerPos = player.transform.position;
        Transform  cameraTransform = Camera.main.transform;
        if(playerPos.x != cameraTransform.position.x || playerPos.y != cameraTransform.position.y)
            cameraTransform.position = new Vector3(playerPos.x, playerPos.y, cameraTransform.position.z);
    }

    Vector3 move(Vector3 objectPos, Vector3 targetPos)
    {
        Vector3Int targetCell = tilemap.WorldToCell(targetPos);
        TileBase targetTile = tilemap.GetTile(targetCell);
        Vector3 targetPosFix = tilemap.GetCellCenterWorld(targetCell);
        
        Vector3Int objectCell = tilemap.WorldToCell(objectPos);
        TileBase objectTile = tilemap.GetTile(objectCell);

        // if (objectTile != null)
        // {
        //     Debug.Log($"Target: {targetPosFix}: {objectTile.name}");
        // } else{
        //     Debug.Log($"Target: {targetPosFix}: vacio");;
        // }

        if(targetTile == null) {
            return objectPos;
        }
        else 
        if(targetTile.name == "Floor") {
            player.transform.position = targetPosFix;
            return targetPos;
        } 
        else if (targetTile.name == "Wall"
            && (objectTile.name == "Wall" || objectTile.name == "Stair")){
            player.transform.position = targetPosFix;
            return targetPos;
        }
        else if (targetTile.name == "Stair"){
            player.transform.position = targetPosFix;
            return targetPos;
        }
        else {
            return objectPos;
        }
        
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.a
    /// </summary>
    void Update()
    {
        Vector3  playerPos = player.transform.position;

        if (Input.GetKeyDown(KeyCode.W)) 
        {
            Vector3 targetPos = new Vector3(playerPos.x, playerPos.y+1, 0);
            playerPos = move(playerPos, targetPos);
        }
        else if (Input.GetKeyDown(KeyCode.S)) 
        {
            Vector3 targetPos = new Vector3(playerPos.x, playerPos.y-1, 0);
            playerPos = move(playerPos, targetPos);
        }
        else if (Input.GetKeyDown(KeyCode.D)) 
        {
            Vector3 targetPos = new Vector3(playerPos.x+1, playerPos.y, 0);
            playerPos = move(playerPos, targetPos);
        }
        else if (Input.GetKeyDown(KeyCode.A)) 
        {
            Vector3 targetPos = new Vector3(playerPos.x-1, playerPos.y, 0);
            playerPos = move(playerPos, targetPos);
        }
        updateCamera();
    }
}
