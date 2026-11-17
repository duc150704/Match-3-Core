using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardView : MonoBehaviour
{
    [Header("Time")]
    [SerializeField] private float _swapTime;
    [SerializeField] private float _removeTime;
    [SerializeField] private float _fallTime;

    [Header("Board")]
    [SerializeField] private float _cellSize;
    [SerializeField] private Vector3 _centerPosition;

    [Header("")]
    [SerializeField] private BoardInput _boardInput;
    [SerializeField] private DiamondView _diamondPref;


    private int _height;
    private int _width;

    public int Height => _height;
    public int Width => _width;
    public float CellSize => _cellSize;

    private CoordinateConverter _converter;
    private List<DiamondView> _views = new();

    public CoordinateConverter Converter => _converter;

    public event Action<GridPosition, GridPosition> OnSwapped;
    public event Action OnDestroyed;

    private void Awake()
    {
        _boardInput.Active = true;

        _boardInput.OnPointerReleased += HandleInput;

    }

    private void OnDestroy()
    {
        _boardInput.OnPointerReleased -= HandleInput;

        OnDestroyed?.Invoke();
    }

    public void Initialize(int height, int width)
    {
        _height = height;
        _width = width;

        _converter = new CoordinateConverter(_cellSize, _centerPosition, Height, Width);
    }

    public DiamondView Create(DiamondDataSO data, GridPosition position)
    {
        Vector3 spawnPosition = _converter.GridToWorld(position.Row, position.Column);

        DiamondView obj = Instantiate(_diamondPref, spawnPosition, Quaternion.identity);
        obj.SetData(data);
        _views.Add(obj);

        return obj;
    }

    public async UniTask Refill(DiamondDataSO data, GridPosition spawnPosition, GridPosition target)
    {
        DiamondView view = Create(data, spawnPosition);
        await FallDown(view, target);
    }

    public async UniTask FallDown(DiamondView view, GridPosition target)
    {
        Vector3 targetPos = _converter.GridToWorld(target.Row, target.Column);
        await view.Move(targetPos, _fallTime);
    }

    public async UniTask Remove(HashSet<DiamondView> views)
    {
        List<UniTask> tasks = new List<UniTask>();
        foreach (var view in views)
        {
            tasks.Add(Remove(view));
        }

        await UniTask.WhenAll(tasks);
    }

    public async UniTask Remove(DiamondView view)
    {
        await view.Scale(Vector3.zero, _removeTime);
        view.DestroySelf() ;
        _views.Remove(view);
    }

    public async UniTask Swap(DiamondView from, DiamondView to)
    {
        Vector3 fromPosition = from.transform.position;
        Vector3 toPosition = to.transform.position;

        await UniTask.WhenAll(
            from.Move(toPosition, _swapTime), 
            to.Move(fromPosition, _swapTime)
            );
    }


    public void ShowAll()
    {
        foreach (var item in _views)
        {
            item.Show();
        }
    }

    public void HideAll()
    {
        foreach (var item in _views)
        {
            item.Hide();
        }
    }

    public void HandleInput(Vector3 from, Vector3 to)
    {
        if ((to - from).magnitude <= 0.5f)
            return;

        GridPosition clicked = _converter.WorldToGrid(from);
        GridPosition target = clicked + DirectionDetectior.Detect(from, to);

        Debug.Log(clicked + " -> " + target);

        OnSwapped?.Invoke(clicked, target);
    }
}
