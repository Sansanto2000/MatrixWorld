using UnityEngine;
using System.Collections.Generic;

public class PieceData
{
    public int code { get; }    
    public char small { get; }
    public string name { get; }
    public GameObject tile { get; set; }

    public PieceData(int code, char small, string name)
    {
        this.code = code;
        this.small = small;
        this.name = name;
    }
}

public class PieceDict
{
    public static readonly PieceData Void = new PieceData(0, '_', "Void");
    public static readonly PieceData Player = new PieceData(1, 'P', "Player");
    public static readonly PieceData Wall = new PieceData(2, 'W', "Wall");
    public static readonly PieceData Stair = new PieceData(3, 'S', "Stair");

    Dictionary<int, PieceData> dict;

    public PieceDict(GameObject[] tiles)
    {
        this.dict = new Dictionary<int, PieceData>();
        this.dict.Add(0, PieceDict.Void);
        this.dict.Add(1, PieceDict.Player);
        this.dict.Add(2, PieceDict.Wall);
        this.dict.Add(3, PieceDict.Stair);
        for (int i = 0; i < tiles.Length; i++)
        {
            this.dict[i].tile = tiles[i];
        }
    }

    public PieceData getPiece(int code){
        return this.dict[code];
    }
}