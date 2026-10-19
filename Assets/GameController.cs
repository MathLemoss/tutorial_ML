public static class GameController
{
    public enum MatchState { Ready, Playing, Won, Lost }

    public static MatchState State { get; private set; }
    public static int TotalCollectables { get; private set; }
    public static int Collected { get; private set; }
    public static int Score => Collected * 100;
    public static float ElapsedTime { get; private set; }
    public static bool gameOver => State == MatchState.Won || State == MatchState.Lost;

    public static void Init(int totalCollectables)
    {
        TotalCollectables = totalCollectables;
        Collected = 0;
        ElapsedTime = 0f;
        State = MatchState.Ready;
    }

    public static void Begin()
    {
        if (State == MatchState.Ready)
            State = MatchState.Playing;
    }

    public static void Tick(float deltaTime)
    {
        if (State == MatchState.Playing && deltaTime > 0f)
            ElapsedTime += deltaTime;
    }

    public static void Collect()
    {
        if (State == MatchState.Playing && Collected < TotalCollectables)
            Collected++;
    }

    public static void Win()
    {
        if (State == MatchState.Playing)
            State = MatchState.Won;
    }

    public static void Lose()
    {
        if (State == MatchState.Playing)
            State = MatchState.Lost;
    }
}
