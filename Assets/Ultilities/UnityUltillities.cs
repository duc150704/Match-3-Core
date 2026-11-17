using UnityEngine;

public static class UnityUltillities
{
    public static int Random(int minInclusive, int maxExclusive)
    {
        return UnityEngine.Random.Range(minInclusive, maxExclusive);
    }
}
