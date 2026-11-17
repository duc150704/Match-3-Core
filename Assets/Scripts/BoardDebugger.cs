using UnityEngine;

public class BoardDebugger : MonoBehaviour
{
    [SerializeField] private bool _showDebug;
    [SerializeField] private BoardView _view;

    private void Update()
    {
        if (!_showDebug || _view == null || _view.Converter == null)
            return;

        Vector3 offset = new Vector3(1, 1) * _view.CellSize / 2;

        for (int i = 0; i <= _view.Height; i++)
        {
            Debug.DrawLine(
                _view.Converter.GridToWorld(i, 0) - offset ,
                _view.Converter.GridToWorld(i, _view.Width) - offset,
                Color.red
                );
        }
        
        for (int i = 0; i <= _view.Width; i++)
        {
            Debug.DrawLine(
                _view.Converter.GridToWorld(0, i) - offset,
                _view.Converter.GridToWorld(_view.Height, i) - offset,
                Color.red
                );
        }
    }
}
