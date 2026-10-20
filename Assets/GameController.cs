public static class GameController
{
    public enum MatchState { Ready, Playing, Won, Lost }
    public enum LossReason { None, Wall, MissedNotes }

    public static MatchState State { get; private set; }
    public static int TotalCollectables { get; private set; }
    public static int Collected { get; private set; }
    public static int ConsecutiveMisses { get; private set; }
    public static LossReason CauseOfLoss { get; private set; }
    public static int Score => Collected * 100;
    public static float ElapsedTime { get; private set; }
    public static bool gameOver => State == MatchState.Won || State == MatchState.Lost;

    public static void Init(int totalCollectables)
    {
        TotalCollectables = totalCollectables;
        Collected = 0;
        ConsecutiveMisses = 0;
        CauseOfLoss = LossReason.None;
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
        if (State != MatchState.Playing)
            return;
        ConsecutiveMisses = 0;
        if (Collected < TotalCollectables)
            Collected++;
    }

    public static void MissNote()
    {
        if (State != MatchState.Playing)
            return;
        ConsecutiveMisses++;
        if (ConsecutiveMisses >= 2)
            Lose(LossReason.MissedNotes);
    }

    public static void RegisterCollectable()
    {
        TotalCollectables++;
    }

    public static void Win()
    {
        if (State == MatchState.Playing)
            State = MatchState.Won;
    }

    public static void Lose(LossReason reason = LossReason.Wall)
    {
        if (State == MatchState.Playing)
        {
            CauseOfLoss = reason;
            State = MatchState.Lost;
        }
    }
}
