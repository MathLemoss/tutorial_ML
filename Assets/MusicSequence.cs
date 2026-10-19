using System;

public sealed class MusicSequence
{
    private readonly double[] durations;
    private readonly double[] starts;
    private readonly double[] sourceEnds = new double[2];
    private int scheduled;
    private double lastEnd;

    public int Released { get; private set; }
    public int Count => durations.Length;
    public double Duration { get; }

    public MusicSequence(double[] clipDurations)
    {
        if (clipDurations == null || clipDurations.Length == 0)
            throw new ArgumentException("A sequencia precisa de pelo menos um trecho.");

        durations = (double[])clipDurations.Clone();
        starts = new double[durations.Length];
        for (int i = 0; i < durations.Length; i++)
        {
            if (durations[i] <= 0 || double.IsNaN(durations[i]) || double.IsInfinity(durations[i]))
                throw new ArgumentException("Duracao de trecho invalida.");
            Duration += durations[i];
        }
    }

    public void ReleaseNext()
    {
        if (Released < Count)
            Released++;
    }

    public bool TrySchedule(double now, out int clip, out int source, out double start, out double end)
    {
        clip = source = -1;
        start = end = 0;
        if (scheduled >= Released)
            return false;

        for (int i = 0; i < sourceEnds.Length; i++)
        {
            if (sourceEnds[i] > now)
                continue;

            clip = scheduled++;
            source = i;
            start = Math.Max(lastEnd, now + 0.05);
            end = start + durations[clip];
            starts[clip] = start;
            sourceEnds[i] = lastEnd = end;
            return true;
        }
        return false;
    }

    public double PlayedSeconds(double now)
    {
        double played = 0;
        for (int i = 0; i < scheduled; i++)
            played += Math.Max(0, Math.Min(durations[i], now - starts[i]));
        return played;
    }

    public bool IsWaiting(double now) => scheduled == Released && now >= lastEnd;
    public bool IsComplete(double now) => scheduled == Count && now >= lastEnd;
}
