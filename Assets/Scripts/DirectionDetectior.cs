using UnityEngine;

public static class DirectionDetectior
{
    public static GridPosition Detect(Vector3 clicked, Vector3 released)
    {
        Vector3 direction = released - clicked;
        direction.Normalize();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        GridPosition res = GridPosition.Zero;
        if (-30 <= angle && angle <= 30)
            res = GridPosition.Right;
        else if (60 <= angle && angle <= 120)
            res = GridPosition.Up;
        else if (-120 <= angle && angle <= -60)
            res = GridPosition.Down;
        else if ((150 <= angle && angle <= 180) || (-180 <= angle && angle <= -150))
            res = GridPosition.Left;

        return res;
    }
}
