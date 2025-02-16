using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GameMaster: MonoBehaviour
{
    [Header("Configuración de celdas")]
    [Tooltip("Celda que representa al jugador.")]
    public GameObject playerTile;

    [Tooltip("Celda que representa al enemigo, maniqui golpeable.")]
    public GameObject hittableDummy;

    [Header("Tilemaps")]
    [Tooltip("Tilemap del mundo.")]
    public Tilemap worldTilemap;

    [Tooltip("Tilemap del entidades.")]
    public Tilemap entityTilemap;

    [Tooltip("Tilemap de elementos lógicos.")]
    public Tilemap logicTilemap;

    [Header("Camera")]
    [Tooltip("Shake script")]
    public CameraShake cameraShake;

    [Tooltip("Quiet")]
    public bool cameraQuiet = false;

    private PieceDict pieceDict;

    private GameObject player;
    private List<GameObject> enemies = new List<GameObject>();

    private TileBase[,] tiles;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        GetAllTilesAndPositions();
        GetEntitiesTiles();
    }

    
    void GetAllTilesAndPositions()
    {
        BoundsInt bounds = worldTilemap.cellBounds;
        int width = bounds.xMax - bounds.xMin;
        int height = bounds.yMax - bounds.yMin;
        tiles = new TileBase[width, height];

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);
                TileBase tile = worldTilemap.GetTile(cellPosition);
                int adjustedX = x - bounds.xMin;
                int adjustedY = y - bounds.yMin;
                tiles[adjustedX, adjustedY] = tile;
            }
        }
    }

    void GetEntitiesTiles()
    {
        BoundsInt bounds = entityTilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);
                TileBase tileBase = entityTilemap.GetTile(cellPosition);
                if (tileBase == null){
                    continue;
                }
                else if (tileBase.name == "Player") 
                {
                    if (tileBase is Tile tile)
                    {
                        Vector3 originPosFix = entityTilemap.GetCellCenterWorld(cellPosition);

                        GameObject obj = new GameObject("Player");
                        obj.transform.position = originPosFix;
                        SpriteRenderer spriteRenderer = obj.AddComponent<SpriteRenderer>();
                        spriteRenderer.sprite = tile.sprite;
                        spriteRenderer.sortingLayerName = entityTilemap.GetComponent<TilemapRenderer>().sortingLayerName;
                
                        player = obj;

                        entityTilemap.SetTile(cellPosition, null);
                    }
                }
                else if (tileBase.name == "HittableDummy") {
                    if (tileBase is Tile tile)
                    {
                        Vector3 originPosFix = entityTilemap.GetCellCenterWorld(cellPosition);

                        GameObject obj = Instantiate(hittableDummy, originPosFix, Quaternion.identity);

                        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
                        spriteRenderer.sortingLayerName = entityTilemap.GetComponent<TilemapRenderer>().sortingLayerName;
                
                        enemies.Add(obj);

                        entityTilemap.SetTile(cellPosition, null);
                    }
                }
            }
        }
        
        if(player == null){
            Debug.LogWarning("Jugador no encontrado en el Tilemap.");
        }
    }
    
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
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

    GameObject checkEnemiesPrecense(Vector3 position) {
        for (int i = 0; i < enemies.Count; i++) {
            if (enemies[i].transform.position == position) {
                return enemies[i];
            }
        }
        return null;
    }

    (Vector3 newPosition, bool moved) move(Vector3 objectPos, Vector3 targetPos)
    {
        GameObject enemyFounded = checkEnemiesPrecense(targetPos);
        if (enemyFounded != null){
            Hittable enemyHittable = enemyFounded.GetComponent<Hittable>();
            if (enemyHittable == null) {
                Debug.Log("El enemigo no es golpeable");
            } 
            else {
                enemyHittable.Hit();
                StartCoroutine(cameraShake.Shake());
            }
            return (objectPos, false);
        }

        Vector3Int targetCell = worldTilemap.WorldToCell(targetPos);
        TileBase targetTile = worldTilemap.GetTile(targetCell);
        Vector3 targetPosFix = worldTilemap.GetCellCenterWorld(targetCell);
        
        Vector3Int objectCell = worldTilemap.WorldToCell(objectPos);
        TileBase objectTile = worldTilemap.GetTile(objectCell);

        if(targetTile == null) {
            return (objectPos, false);
        }
        else 
        if(targetTile.name == "Floor") {
            player.transform.position = targetPosFix;
            return (targetPos, true);
        } 
        else if (targetTile.name == "Wall"
            && (objectTile.name == "Wall" || objectTile.name == "Stair")){
            player.transform.position = targetPosFix;
            return (targetPos, true);
        }
        else if (targetTile.name == "Stair"){
            player.transform.position = targetPosFix;
            return (targetPos, true);
        }
        else {
            return (objectPos, false);
        }
        
    }

    void checkLogic(Vector3 objectPos){
        Vector3Int objectCell = logicTilemap.WorldToCell(objectPos);
        TileBase objectTile = logicTilemap.GetTile(objectCell);

        if(objectTile == null) {
            return;
        }
        else 
        if(objectTile.name == "Checkpoint") {
            Debug.Log("Checkpoint alcanzado");
            return;
        } 
        else {
            return;
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.a
    /// </summary>
        void Update()
        {
            Vector3  playerPos = player.transform.position;
            Vector3 targetPos = playerPos;

            switch(true){
                case var _ when Input.GetKeyDown(KeyCode.W):
                    targetPos.y += 1;
                    break;
                case var _ when Input.GetKeyDown(KeyCode.S):
                    targetPos.y -= 1;
                    break;
                case var _ when Input.GetKeyDown(KeyCode.D):
                    targetPos.x += 1;
                    break;
                case var _ when Input.GetKeyDown(KeyCode.A):
                    targetPos.x -= 1;
                    break;
            }

            bool moved = false;
            if(targetPos != playerPos){
                (playerPos, moved) = move(playerPos, targetPos);
            }
            
            if (moved){
                checkLogic(playerPos);
            }
            updateCamera();
        }
}
