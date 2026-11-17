using UnityEngine;

[CreateAssetMenu(menuName ="Diamond Database")]
public class DiamondDataBaseSO : ScriptableObject
{
    [SerializeField] private DiamondDataSO[] _data;

    public DiamondDataSO[] Data => _data;
    public int Count => _data.Length;

    public DiamondDataSO Get(int type)
        => _data[type];

    public DiamondDataSO GetRandomData()
    {
        int type = GetRandomType();
        return _data[type];
    }

    public int GetRandomType()
        => UnityUltillities.Random(0, Count);
}
