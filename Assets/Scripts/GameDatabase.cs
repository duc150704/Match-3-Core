using UnityEngine;

public class GameDatabase : Singleton<GameDatabase>
{
    [SerializeField] private DiamondDataBaseSO _diamondDataBaseSO;

    public DiamondDataBaseSO DiamondDataBaseSO => _diamondDataBaseSO;
}
