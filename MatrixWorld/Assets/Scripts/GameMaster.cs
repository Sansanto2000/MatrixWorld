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

    int[,] world = {
        {0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0},
        {0,0,0,1,0,0,0},
        {0,0,0,0,0,0,0},
    };

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        for (int i = 0; i < world.GetLength(0); i++)
        {
            for (int j = 0; j < world.GetLength(1); j++)
            {
                GameObject tile;
                if(world[i,j] == WorldPiece.Player.code) {
                    tile = playerTile;
                } else if (world[i,j] == WorldPiece.Void.code) {
                    tile = voidTile;
                } else {
                    throw new KeyNotFoundException($"El código de celda especificado ({world[i,j]}) no esta corresponde a ningún tipo de celda aceptado.");
                }
                Instantiate(tile, new Vector2(j, -i), Quaternion.identity);
            }
        }
    }
}
