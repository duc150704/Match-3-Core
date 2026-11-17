using System.Collections.Generic;

public enum BoardDirection
{
    Vertical, Horizontal
}

public class Match3Logic
{
    private Board _board;

    public Match3Logic(Board board)
    {
        _board = board;
    }

    public FillResult Fill()
    {
        RefillablePositions rP = FindRefillablePosition();
        FillResult fiR = new FillResult();

        foreach (var pos in rP.Positions)
        {
            int type = GameDatabase.Instance.DiamondDataBaseSO.GetRandomType();
            Diamond d = new Diamond(type, pos.RefillablePosition);
            _board.Set(pos.RefillablePosition.Row, pos.RefillablePosition.Column, d);

            FillResult.Fill fill = new() 
            { 
                Diamond = d,
                SuggestSpawnPosition = pos.SuggestSpawnPosition,
            };
            fiR.Fills.Add(fill);
        }

        return fiR;
    }

    public RefillablePositions FindRefillablePosition()
    {
        RefillablePositions result = new();

        int column = 0;
        while (column < _board.Width)
        {
            int up = 0;
            while (up < _board.Height)
            {
                if (!_board.IsEmpty(up, column))
                {
                    up++;
                    continue;
                }

                GridPosition minPos = new GridPosition(up, column);
                GridPosition height = new GridPosition(_board.Height, column);

                for (int i = up; i < _board.Height; i++)
                {
                    GridPosition currentPos = new GridPosition(i, column);
                    RefillablePositions.RefillPosition pos = new()
                    {
                        RefillablePosition = currentPos,
                        SuggestSpawnPosition = currentPos + (height - minPos)
                    };
                    result.Positions.Add(pos);
                }

                break;
            }
            column++;
        }

        return result;
    }

    public FallResult FallDown()
    {
        FallResult fR = new FallResult();

        int column = 0;
        while(column < _board.Width)
        {
            int down = 0;
            int up = 0;

            while(up < _board.Height && down < _board.Height)
            {
                if (up == down)
                {
                    up++;
                    continue;
                }

                if (!_board.IsEmpty(down, column))
                {
                    down++;
                    up = down;
                    continue;
                }

                if (_board.IsEmpty(up, column))
                {
                    up++;
                    continue;
                }

                Diamond upDiamond = _board.Get(up, column);
                _board.Set(down, column, upDiamond);
                _board.Set(up, column, null);

                fR.Falls.Add(
                    new Fall(
                        upDiamond, 
                        new GridPosition(up, column), 
                        new GridPosition(down, column)
                        )
                    );

                down++;
                up = down + 1;
            }

            column++;
        }

        return fR;
    }

    public RemoveResult Remove(HashSet<GridPosition> positions)
    {
        RemoveResult rR = new RemoveResult();
        foreach (var pos in positions)
        {
            Diamond obj = _board.Get(pos.Row, pos.Column);
            rR.RemovedDiamonds.Add(obj);

            _board.Set(pos.Row, pos.Column, null);
        }

        return rR;
    }

    public SwapResult Swap(GridPosition from, GridPosition to)
    {
        SwapResult  sR = new SwapResult();

        if (_board.IsEmpty(from.Row, from.Column) 
            || _board.IsEmpty(to.Row, to.Column)
            || from == to)
        {
            sR.Success = false;
            return sR;
        }

        Diamond fDiamond = _board.Get(from.Row, from.Column);
        Diamond tDiamond = _board.Get(to.Row, to.Column);

        _board.Set(to.Row, to.Column, fDiamond);
        _board.Set(from.Row, from.Column, tDiamond);

        sR.FromPosition = from;
        sR.TargetPosition = to;
        sR.From = fDiamond;
        sR.To = tDiamond;
        sR.Success = true;

        return sR;
    }

    public bool CanSwap(GridPosition from, GridPosition to)
    {
        if (_board.IsEmpty(from.Row, from.Column) || _board.IsEmpty(to.Row, to.Column))
            return false;

        Diamond fDiamond = _board.Get(from.Row, from.Column);
        Diamond tDiamond = _board.Get(to.Row, to.Column);

        _board.Set(to.Row, to.Column, fDiamond);
        _board.Set(from.Row, from.Column, tDiamond);

        MatchResult mR = Scan();

        _board.Set(to.Row, to.Column, tDiamond);
        _board.Set(from.Row, from.Column, fDiamond);

        return mR.HasMatches;
    }

    public MatchResult Scan()
    {
        MatchResult mR = new MatchResult();

        mR.Append(VerticalScan());
        mR.Append(HorizontalScan());

        return mR;
    }

    private MatchResult HorizontalScan()
    {
        MatchResult mR = new MatchResult();

        int row = 0;
        while (row < _board.Height) 
        {
            int right = 0;
            int left = 0;
            while(right < _board.Width)
            {
                if (left == right)
                {
                    right++;
                    continue;
                }

                Diamond lDiamond = _board.Get(row, left);
                Diamond rDiamond = _board.Get(row, right);

                if (lDiamond.Type != rDiamond.Type)
                {
                    if (right - left >= 3)
                    {
                        AddResult(mR, left, right - 1, BoardDirection.Horizontal, row);
                    }

                    left = right;
                    right++;
                }
                else
                {
                    right++;
                }
            }

            if (right - left >= 3)
            {
                AddResult(mR, left, right - 1, BoardDirection.Horizontal, row);
            }

            row++;
        }

        return mR;
    }

    private MatchResult VerticalScan()
    {
        MatchResult mR = new MatchResult();

        int column = 0;
        while (column < _board.Width)
        {
            int up = 0;
            int down = 0;
            while (up < _board.Height)
            {
                if (up == down)
                {
                    up++;
                    continue;
                }

                Diamond dDiamond = _board.Get(down, column);
                Diamond uDiamond = _board.Get(up, column);

                if (uDiamond.Type != dDiamond.Type)
                {
                    if (up - down >= 3)
                    {
                        AddResult(mR, down, up - 1, BoardDirection.Vertical, column);
                    }

                    down = up;
                    up++;
                }
                else
                {
                    up++;
                }
            }

            if (up - down >= 3)
            {
                AddResult(mR, down, up - 1, BoardDirection.Vertical, column);
            }

            column++;
        }

        return mR;
    }

    private void AddResult(MatchResult mR, int from, int to, BoardDirection dir, int line)
    {
        int index = (from < to) ? from : to;
        int maxIndex = (from < to) ? to : from;

        while (index <= maxIndex)
        {
            GridPosition pos = (dir == BoardDirection.Vertical)
                ? new GridPosition(index, line) 
                : new GridPosition(line, index);

            mR.MathedPositions.Add(pos);
            index++;
        }
    }
}
