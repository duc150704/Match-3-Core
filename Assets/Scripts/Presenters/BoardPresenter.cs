using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

public class BoardPresenter : IDisposable
{
    private Dictionary<Diamond, DiamondView> _modelToViewMap = new();
    private Board _boardModel;
    private BoardView _boardView;
    private Match3Logic _match3Logic;

    public BoardPresenter(Board boardModel, BoardView boardView)
    {
        _boardModel = boardModel;
        _boardView = boardView;

        _match3Logic = new Match3Logic(boardModel);
        _boardView.Initialize(boardModel.Height, _boardModel.Width);

        RegisterEvent();
    }

    public void Dispose()
    {
        UnregisterEvent();
    }

    public void StartGame()
    {
        SpawnDiamond();
    }

    private void SpawnDiamond()
    {
        foreach (var pos in _boardModel.Positions())
        {
            DiamondDataSO data = GameDatabase.Instance.DiamondDataBaseSO.GetRandomData();
            Diamond obj = new Diamond(data.Type, pos);

            _boardModel.Set(pos.Row, pos.Column, obj);

            DiamondView view = _boardView.Create(data, pos);

            _modelToViewMap[obj] = view;
        }
    }

    private async void SwapHandler(GridPosition from, GridPosition target)
    {
        if (!_boardModel.IsInside(from.Row, from.Column) || !_boardModel.IsInside(target.Row, target.Column))
            return;

        SwapResult sR = _match3Logic.Swap(from, target);

        if (sR.Success == false)
            return;

        await _boardView.Swap(_modelToViewMap[sR.From], _modelToViewMap[sR.To]);

        MatchResult mR = _match3Logic.Scan();
        if (!mR.HasMatches)
        {
            _match3Logic.Swap(from, target);
            await UniTask.Delay(100);
            await _boardView.Swap(_modelToViewMap[sR.From], _modelToViewMap[sR.To]);
            return;
        }

        RemoveResult rR = _match3Logic.Remove(mR.MathedPositions);


        HashSet<DiamondView> removeDiamond = new();
        foreach (var item in rR.RemovedDiamonds)
        {
            removeDiamond.Add(_modelToViewMap[item]);
            _modelToViewMap.Remove(item);
        }

        await _boardView.Remove(removeDiamond);
        
        FallResult fR = _match3Logic.FallDown();

        foreach (var item in fR.Falls)
        {
            _boardView.FallDown(_modelToViewMap[item.Diamond], item.To).Forget();
        }

        FillResult fiR = _match3Logic.Fill();

        

        foreach (var item in fiR.Fills)
        {
            DiamondDataSO data = GameDatabase.Instance.DiamondDataBaseSO.Get(item.Diamond.Type);
            DiamondView view = _boardView.Create(data, item.SuggestSpawnPosition);
            _modelToViewMap[item.Diamond] = view;
            _boardView.FallDown(view, item.Diamond.Position).Forget();
        }

    }

    private void RegisterEvent()
    {
        _boardView.OnSwapped += SwapHandler;
        _boardView.OnDestroyed += Dispose;
    }
    
    private void UnregisterEvent()
    {
        _boardView.OnSwapped -= SwapHandler;
        _boardView.OnDestroyed -= Dispose;
    }
}
