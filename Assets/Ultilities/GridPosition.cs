using System;

[Serializable]
public struct GridPosition
{
    public int Row;
    public int Column;

    public static readonly GridPosition Zero = new GridPosition(0, 0);
    public static readonly GridPosition Up = new GridPosition(1, 0);
    public static readonly GridPosition Down = new GridPosition(-1, 0);
    public static readonly GridPosition Left = new GridPosition(0, -1);
    public static readonly GridPosition Right = new GridPosition(0, 1);

    public GridPosition(int row, int column)
    {
        Row = row;
        Column = column;
    }

    public override string ToString()
    {
        return $"{Row} , {Column}";
    }

    public static GridPosition operator +(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.Row + b.Row, a.Column + b.Column) ;
    }
    
    public static GridPosition operator -(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.Row - b.Row, a.Column - b.Column) ;
    }

    public static bool operator ==(GridPosition a, GridPosition b) 
    { 
        return a.Row == b.Row && a.Column == b.Column ;
    }
    
    public static bool operator !=(GridPosition a, GridPosition b) 
    { 
        return a.Row != b.Row || a.Column != b.Column ;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Row, Column);
    }

    public override bool Equals(object obj)
    {
        return obj is GridPosition other 
            && Row == other.Row && Column == other.Column ;
    }
}
