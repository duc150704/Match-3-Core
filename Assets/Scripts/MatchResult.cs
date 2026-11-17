using System.Collections.Generic;

public class FillResult
{
    public List<Fill> Fills { get; } = new();

    public class Fill
    {
        public Diamond Diamond { get; set; }
        public GridPosition SuggestSpawnPosition { get; set; }
    }
}

public class RefillablePositions
{
    public List<RefillPosition> Positions { get; } = new();

    public struct RefillPosition
    {
        public GridPosition RefillablePosition;
        public GridPosition SuggestSpawnPosition;
    }

    public List<GridPosition> GetSuggestSpawnPositions()
    {
        List<GridPosition> positions = new();
        foreach (var item in Positions)
        {
            positions.Add(item.SuggestSpawnPosition);
        }

        return positions;
    }

    public List<GridPosition> GetFillPositions()
    {
        List<GridPosition> positions = new();
        foreach (var item in Positions)
        {
            positions.Add(item.RefillablePosition);
        }

        return positions;
    }
}

public class FallResult
{
    public List<Fall> Falls { get; } = new();
}

public class Fall
{
    public Diamond Diamond { get; private set; }
    public GridPosition From { get; private set; }
    public GridPosition To { get; private set; }

    public Fall(Diamond diamond, GridPosition from, GridPosition to)
    {
        Diamond = diamond;
        From = from;
        To = to;
    }
}

public class RemoveResult
{
    public HashSet<Diamond> RemovedDiamonds { get; } = new();
}

public struct SwapResult
{
    public bool Success;
    public GridPosition FromPosition;
    public GridPosition TargetPosition;
    public Diamond From;
    public Diamond To;
}

public class MatchResult 
{
    public HashSet<GridPosition> MathedPositions { get; } = new(); 
    public bool HasMatches => MathedPositions.Count > 0;

    public void Append(MatchResult other)
    {
        this.MathedPositions.UnionWith(other.MathedPositions);
    }
}
