public class Diamond 
{
    public int Type { get; }
    public GridPosition Position { get; set; }

    public Diamond(int type, GridPosition position)
    {
        Type = type;
        Position = position;
    }   
}
