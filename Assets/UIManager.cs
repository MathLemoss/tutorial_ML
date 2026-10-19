using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject endGamePanel;
    public TMP_Text resultText;
    public TMP_Text hudText;
    public TMP_Text instructionsText;
    public ZigZagLevel level;

    void Awake()
    {
        endGamePanel.SetActive(false);
    }

    void Update()
    {
        bool hasMusic = level.music != null && level.music.HasTrack;
        string progressLabel = hasMusic ? "Musica" : "Percurso";
        hudText.text = $"Pontos: {GameController.Score}    Notas: {GameController.Collected}"
            + $"\nTempo: {GameController.ElapsedTime:0.0}s    {progressLabel}: {Mathf.FloorToInt(level.Progress * 100f)}%";
        instructionsText.text = GameController.State == GameController.MatchState.Ready
            ? "ESPACO / A para comecar\nPegue as notas para liberar a musica. Evite as paredes!"
            : hasMusic && level.music.IsWaiting
                ? "Pegue uma nota para continuar a musica!"
                : "ESPACO / A: trocar direcao";
        instructionsText.gameObject.SetActive(!GameController.gameOver);

        if (endGamePanel.activeSelf != GameController.gameOver)
        {
            endGamePanel.SetActive(GameController.gameOver);
            if (GameController.gameOver)
            {
                string title = GameController.State == GameController.MatchState.Won
                    ? (hasMusic ? "Musica completa!" : "Percurso concluido!") : "Marrone te pegou!";
                resultText.text = $"{title}\n<size=65%>Tempo: {GameController.ElapsedTime:0.0}s | Pontos: {GameController.Score}</size>";
                if (EventSystem.current != null)
                    EventSystem.current.SetSelectedGameObject(endGamePanel.GetComponentInChildren<Button>().gameObject);
            }
        }
    }
}
