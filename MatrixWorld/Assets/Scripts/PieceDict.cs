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

    Dictionary<int, PieceData> dict;

    public PieceDict(GameObject[] tiles)
    {
        this.dict = new Dictionary<int, PieceData>();
        this.dict.Add(0, PieceDict.Void);
        this.dict.Add(1, PieceDict.Player);
        for (int i = 0; i < tiles.Length; i++)
        {
            this.dict[i].tile = tiles[i];
        }
    }

    public PieceData getPiece(int code){
        return this.dict[code];
    }
}