using UnityEngine;

[CreateAssetMenu(menuName ="Diamond")]
public class DiamondDataSO : ScriptableObject
{
    [SerializeField] private int _type;
    [SerializeField] private Sprite _sprite;

    public int Type => _type;
    public Sprite Sprite => _sprite;
}
