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