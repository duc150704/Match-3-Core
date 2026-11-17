using System.Collections.Generic;
using System.Data;

public class Board 
{
    private Grid2D<Diamond> _grid;

    public int Height => _grid.Rows;
    public int Width => _grid.Columns;

    public Board(int height, int width)
    {
        _grid = new Grid2D<Diamond>(height, width);
    }

    public bool Set(int row, int column, Diamond value)
    {
        GridPosition placePos = new GridPosition(row, column);
        if (!_grid.Set(placePos, value))
            return false;

        if (value != null)
            value.Position = placePos;

        return true;
    }

    public Diamond Get(int row, int column) 
    { 
        GridPosition pos = new GridPosition(row, column);
        return _grid.Get(pos);
    }

    public bool IsEmpty(int row, int column)
    {
        GridPosition pos = new GridPosition(row, column);
        if (!_grid.IsValid(pos))
            return true;

        return _grid.Get(pos) == null;
    }

    public bool IsInside(int row, int column)
    {
        GridPosition pos = new GridPosition(row, column);
        return _grid.IsValid(pos);
    }

    public IEnumerable<Diamond> Diamonds()
    {
        for (int r = 0; r < Height; r++)
            for (int c = 0; c < Width; c++)
                yield return _grid.Get(new GridPosition(r, c));
    }
    
    public IEnumerable<GridPosition> Positions()
    {
        for (int r = 0; r < Height; r++)
            for (int c = 0; c < Width; c++)
                yield return new GridPosition(r, c);
    }

}
