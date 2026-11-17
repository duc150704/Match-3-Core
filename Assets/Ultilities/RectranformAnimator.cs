using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class RectranformAnimator : MonoBehaviour
{
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public Tween ScaleTo(Vector3 targetValue, float duration, Ease ease = Ease.Linear)
    {
        return _rectTransform.DOScale(targetValue, duration).SetEase(ease);
    }

    public Tween MoveTo(Vector3 targetPosition, float duration, Ease ease = Ease.Linear)
    {
        return _rectTransform.DOMove(targetPosition, duration).SetEase(ease);
    }

    public Tween MoveAnchorPositionTo(Vector3 targetPosition, float duration, Ease ease = Ease.Linear)
    {
        return _rectTransform.DOAnchorPos(targetPosition, duration).SetEase(ease);
    }
}
