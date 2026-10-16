using UnityEngine;

public static class GameController
{
    private static int collectableCount;

    public static bool gameOver 
    { 
        get { return collectableCount <= 0; }
    }

    public static void Init(int totalCollectables)
    {
        collectableCount = totalCollectables;
    }

    public static void Collect()
    {
        collectableCount --;
    }
}
