public class Grid2D<T>
{
    public int Rows { get; private set; }
    public int Columns { get; private set; }

    private T[,] _values;

    public Grid2D(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;
        _values = new T[Rows, Columns];
    }

    public bool Set(GridPosition gridPosition, T value)
    {
        if (!IsValid(gridPosition))
            return false;

        _values[gridPosition.Row, gridPosition.Column] = value;
        return true;
    }

    public T Get(GridPosition gridPosition)
    {
        if (!IsValid(gridPosition))
            return default(T);

        return _values[gridPosition.Row, gridPosition.Column];
    }

    public bool IsValid(GridPosition gridPosition)
    {
        return 0 <= gridPosition.Row && gridPosition.Row < Rows
            && 0 <= gridPosition.Column && gridPosition.Column < Columns;
    }
}
