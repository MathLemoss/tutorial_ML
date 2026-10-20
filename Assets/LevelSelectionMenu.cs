using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionMenu : MonoBehaviour
{
    public MusicLevel[] levels;
    public Button[] levelButtons;
    public TMP_Text[] levelLabels;

    public Button PreferredButton
    {
        get
        {
            int selected = Array.IndexOf(levels, SelectedMusicLevel.Current);
            return levelButtons[selected >= 0 ? selected : 0];
        }
    }

    void Awake()
    {
        // A ordem visual e a escolha usam a mesma lista, ordenada pela duracao real.
        Array.Sort(levels, (a, b) => a.durationSeconds.CompareTo(b.durationSeconds));
        for (int i = 0; i < levels.Length; i++)
            levelLabels[i].text = $"{levels[i].songTitle}\n<size=70%>{levels[i].DurationLabel}</size>";
    }

    public void PlayLevel(int index)
    {
        if (index < 0 || index >= levels.Length)
            return;
        SelectedMusicLevel.Current = levels[index];
        SceneManager.LoadScene("SampleScene");
    }
}
