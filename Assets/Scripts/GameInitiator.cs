using UnityEngine;

public class GameInitiator : MonoBehaviour
{
    [SerializeField] private BoardView _boardView;

    private void Start()
    {
        Board board = new Board(5, 6);
        BoardPresenter boardPresenter = new BoardPresenter(board, _boardView);
        boardPresenter.StartGame();
    }
}
