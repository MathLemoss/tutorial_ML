using UnityEngine;

[CreateAssetMenu(fileName = "NovaFase", menuName = "B&M RUN/Fase musical")]
public class MusicLevel : ScriptableObject
{
    public string songTitle;
    [Min(1f)] public float durationSeconds;
    public AudioClip[] clips;

    public string DurationLabel
    {
        get
        {
            int seconds = Mathf.RoundToInt(durationSeconds);
            return $"{seconds / 60}:{seconds % 60:00}";
        }
    }
}

public static class SelectedMusicLevel
{
    public static MusicLevel Current { get; set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Reset() => Current = null;
}
