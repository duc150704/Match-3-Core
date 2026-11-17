using Cysharp.Threading.Tasks;
using UnityEngine;

public class DiamondView : MonoBehaviour
{
    private int _type;
    private TranformAnimator _animator;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<TranformAnimator>();
    }

    public void SetData(DiamondDataSO data)
    {
        _type = data.Type;
        _spriteRenderer.sprite = data.Sprite;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public UniTask Move(Vector3 target, float moveTime)
    {
        return _animator.Move(target, moveTime).ToUniTask();
    }

    public UniTask Scale(Vector3 target, float time)
    {
        return _animator.Scale(target, time).ToUniTask();
    }

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
