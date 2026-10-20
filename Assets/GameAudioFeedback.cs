using UnityEngine;

public class GameAudioFeedback : MonoBehaviour
{
    [SerializeField] private AudioClip suspenseClip = null;
    [SerializeField] private AudioClip victoryClip = null;
    [SerializeField] private AudioClip defeatClip = null;
    [SerializeField, Range(0f, 1f)] private float suspenseVolume = 0.45f;
    [SerializeField, Range(0f, 1f)] private float resultVolume = 0.8f;

    private AudioSource suspenseSource;
    private AudioSource resultSource;
    private SequentialMusicPlayer music;
    private bool warningActive;
    private bool resultPlayed;

    void Awake()
    {
        music = GetComponent<SequentialMusicPlayer>();
        suspenseSource = CreateSource(suspenseVolume, true);
        suspenseSource.clip = suspenseClip;
        resultSource = CreateSource(resultVolume, false);
    }

    AudioSource CreateSource(float volume, bool loop)
    {
        var source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.spatialBlend = 0f;
        source.volume = volume;
        source.loop = loop;
        return source;
    }

    void LateUpdate()
    {
        bool warning = GameController.State == GameController.MatchState.Playing
            && GameController.ConsecutiveMisses == 1;
        if (warning != warningActive)
        {
            warningActive = warning;
            if (warning && suspenseClip != null)
                suspenseSource.Play();
            else
                suspenseSource.Stop();
        }

        if (GameController.State == GameController.MatchState.Ready)
        {
            resultPlayed = false;
            resultSource.Stop();
        }
        else if (GameController.gameOver && !resultPlayed)
        {
            resultPlayed = true;
            if (music != null)
                music.StopPlayback();
            resultSource.clip = GameController.State == GameController.MatchState.Won
                ? victoryClip : defeatClip;
            if (resultSource.clip != null)
                resultSource.Play();
        }
    }

    void OnDisable()
    {
        if (suspenseSource != null)
            suspenseSource.Stop();
        if (resultSource != null)
            resultSource.Stop();
        warningActive = false;
    }
}
