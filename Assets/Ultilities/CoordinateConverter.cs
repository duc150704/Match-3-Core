using UnityEngine;

public class CoordinateConverter
{
    private float _cellSize;
    private int _height;
    private int _width;
    private Vector3 _offset;

    public CoordinateConverter(float cellSize, Vector3 centerPositon, int height, int width)
    {
        _cellSize = cellSize;
        _height = height;
        _width = width;

        CalculateOffset(centerPositon);
    }

    public Vector3 GridToWorld(int row, int column)
    {
        float x = column * _cellSize;
        float y = row * _cellSize;

        return new Vector3(x, y) + new Vector3(_cellSize, _cellSize) / 2 + _offset;
    }

    public GridPosition WorldToGrid(Vector3 position)
    {
        position -= _offset;

        int column = Mathf.FloorToInt(position.x / _cellSize);
        int row = Mathf.FloorToInt(position.y / _cellSize);

        return new GridPosition(row, column);
    }

    private void CalculateOffset(Vector3 centerPosition)
    {
        Vector3 centerGrid = new Vector3(_width, _height) * _cellSize / 2;
        _offset = centerPosition - centerGrid;
    }
}
