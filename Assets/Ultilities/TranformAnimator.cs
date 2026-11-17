using DG.Tweening;
using UnityEngine;

public class TranformAnimator : MonoBehaviour
{
    public Tween Scale(Vector3 target, float duration, Ease ease = Ease.Linear)
    {
        return transform.DOScale(target, duration).SetEase(ease);
    }

    public Tween Move(float distance, Vector3 direction, float duration)
    {
        Vector3 target = direction.normalized * distance;
        return Move(target, duration);
    }

    public Tween Move(Vector3 target, float duration, Ease ease = Ease.Linear)
    {
        return transform.DOMove(target, duration).SetEase(ease);
    }
}
