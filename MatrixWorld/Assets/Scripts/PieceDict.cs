using System.Collections.Generic;

public class PieceData
{
    public int code { get; }    
    public char small { get; }
    public string name { get; }

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

    public PieceDict()
    {
        this.dict = new Dictionary<int, PieceData>();
        this.dict.Add(0, PieceDict.Void);
        this.dict.Add(1, PieceDict.Player);
    }
}