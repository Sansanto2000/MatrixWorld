using UnityEngine;
using System.Collections.Generic;

public class PieceData
{
    public int code { get; }
    public char small { get; }

    public PieceData(int code, char small)
    {
        this.code = code;
        this.small = small;
    }
}

public static class WorldPiece
{
    public static readonly PieceData Void = new PieceData(0, '_');
    public static readonly PieceData Player = new PieceData(1, 'P');
}

public class GameMaster: MonoBehaviour
{
    public GameObject voidTile;
    public GameObject playerTile;

    private int[,] world = {
        {0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0},
        {0,0,0,1,0,0,0},
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
                if(world[i,j] == WorldPiece.Player.code) {
                    tile = playerTile;
                    playerPos = (i, j);
                } else if (world[i,j] == WorldPiece.Void.code) {
                    tile = voidTile;
                } else {
                    throw new KeyNotFoundException($"El código de celda especificado ({world[i,j]}) no esta corresponde a ningún tipo de celda aceptado.");
                }
                Instantiate(tile, new Vector2(j, -i), Quaternion.identity);
            }
        }
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        worldRendering();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) 
        {
            if (playerPos.y - 1 >= 0) {
                if(world[playerPos.y - 1, playerPos.x] == WorldPiece.Void.code) {
                    world[playerPos.y, playerPos.x] = WorldPiece.Void.code;
                    world[playerPos.y - 1, playerPos.x] = WorldPiece.Player.code;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.S)) 
        {
            if (playerPos.y + 1 < world.GetLength(0)) {
                if(world[playerPos.y + 1, playerPos.x ] == WorldPiece.Void.code) {
                    world[playerPos.y, playerPos.x] = WorldPiece.Void.code;
                    world[playerPos.y + 1, playerPos.x] = WorldPiece.Player.code;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.D)) 
        {
            if (playerPos.x + 1 < world.GetLength(1)) {
                if(world[playerPos.y, playerPos.x + 1] == WorldPiece.Void.code) {
                    world[playerPos.y, playerPos.x] = WorldPiece.Void.code;
                    world[playerPos.y, playerPos.x + 1] = WorldPiece.Player.code;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.A)) 
        {
            if (playerPos.x - 1 >= 0) {
                if(world[playerPos.y, playerPos.x - 1] == WorldPiece.Void.code) {
                    world[playerPos.y, playerPos.x] = WorldPiece.Void.code;
                    world[playerPos.y, playerPos.x - 1] = WorldPiece.Player.code;
                }
            }
        }   
        worldRendering();
    }
}
