using UnityEngine;

public class SequentialMusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips = new AudioClip[0];
    [SerializeField] private MusicLevel defaultLevel;
    [SerializeField, Range(0f, 1f)] private float volume = 0.8f;

    private readonly AudioSource[] sources = new AudioSource[2];
    private MusicSequence sequence;
    private bool stopped;
    private double stoppedAt;

    private double Clock => stopped ? stoppedAt : AudioSettings.dspTime;
    public bool HasTrack => sequence != null;
    public int Released => sequence != null ? sequence.Released : 0;
    public int SegmentCount => clips.Length;
    public float Duration => sequence != null ? (float)sequence.Duration : 0f;
    public float Progress => sequence != null ? (float)(sequence.PlayedSeconds(Clock) / sequence.Duration) : 0f;
    public bool IsWaiting => sequence != null && !stopped && sequence.IsWaiting(Clock);
    public bool IsComplete => sequence != null && sequence.IsComplete(Clock);

    void Awake()
    {
        MusicLevel selected = SelectedMusicLevel.Current != null ? SelectedMusicLevel.Current : defaultLevel;
        if (selected != null)
            clips = selected.clips;
        if (clips.Length == 0)
            return;

        double[] durations = new double[clips.Length];
        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i] == null)
            {
                Debug.LogError("Falta um trecho da musica no Inspector.", this);
                return;
            }
            durations[i] = (double)clips[i].samples / clips[i].frequency;
            clips[i].LoadAudioData();
        }
        sequence = new MusicSequence(durations);
        for (int i = 0; i < sources.Length; i++)
        {
            sources[i] = gameObject.AddComponent<AudioSource>();
            sources[i].playOnAwake = false;
            sources[i].loop = false;
            sources[i].spatialBlend = 0f;
            sources[i].volume = volume;
        }
    }

    public void ReleaseNext()
    {
        if (sequence != null && GameController.State == GameController.MatchState.Playing && !stopped)
            sequence.ReleaseNext();
    }

    void Update()
    {
        if (GameController.gameOver)
        {
            StopPlayback();
            return;
        }
        if (sequence == null || stopped || GameController.State != GameController.MatchState.Playing)
            return;

        double now = AudioSettings.dspTime;
        while (sequence.TrySchedule(now, out int clip, out int source, out double start, out double end))
        {
            sources[source].clip = clips[clip];
            sources[source].PlayScheduled(start);
            sources[source].SetScheduledEndTime(end);
        }
    }

    public void StopPlayback()
    {
        if (stopped)
            return;
        stoppedAt = AudioSettings.dspTime;
        stopped = true;
        foreach (AudioSource source in sources)
            if (source != null)
                source.Stop();
    }

    void OnDisable()
    {
        StopPlayback();
    }
}
