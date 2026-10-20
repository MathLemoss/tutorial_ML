using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;

public class UIManager : MonoBehaviour
{
    public GameObject endGamePanel;
    public TMP_Text resultText;
    public TMP_Text hudText;
    public TMP_Text instructionsText;
    public ZigZagLevel level;
    private float defeatElapsed;

    void Awake()
    {
        endGamePanel.SetActive(false);
    }

    void Update()
    {
        bool hasMusic = level.music != null && level.music.HasTrack;
        string progressLabel = hasMusic ? "Musica" : "Percurso";
        hudText.text = $"Pontos: {GameController.Score}    Notas: {GameController.Collected}    Falhas: {GameController.ConsecutiveMisses}/2"
            + $"\nTempo: {GameController.ElapsedTime:0.0}s    {progressLabel}: {Mathf.FloorToInt(level.Progress * 100f)}%";
        instructionsText.text = GameController.State == GameController.MatchState.Ready
            ? "ESPACO / A para comecar\nDuas notas perdidas seguidas ou uma batida: Marrone te pega!"
            : GameController.ConsecutiveMisses == 1
                ? "Marrone esta perto! Pegue a proxima nota para afasta-lo."
                : hasMusic && level.music.IsWaiting
                    ? "Pegue uma nota para continuar a musica!"
                    : "ESPACO / A: trocar direcao";
        instructionsText.gameObject.SetActive(!GameController.gameOver);

        // Deixe a aproximacao final aparecer antes de abrir o resultado.
        if (GameController.State == GameController.MatchState.Lost)
            defeatElapsed += Time.unscaledDeltaTime;
        bool showResult = GameController.gameOver
            && (GameController.State == GameController.MatchState.Won || defeatElapsed >= 0.4f);
        if (endGamePanel.activeSelf != showResult)
        {
            endGamePanel.SetActive(showResult);
            if (showResult)
            {
                string title = GameController.State == GameController.MatchState.Won
                    ? (hasMusic ? "Musica completa!" : "Percurso concluido!") : "Marrone te pegou!";
                string reason = GameController.CauseOfLoss == GameController.LossReason.MissedNotes
                    ? "Duas notas perdidas seguidas." : "Voce bateu na parede.";
                string detail = GameController.State == GameController.MatchState.Lost ? reason + "\n" : "";
                resultText.text = $"{title}\n<size=65%>{detail}Tempo: {GameController.ElapsedTime:0.0}s | Pontos: {GameController.Score}</size>";
                if (EventSystem.current != null)
                {
                    var input = EventSystem.current.GetComponent<InputSystemUIInputModule>();
                    if (input != null)
                        input.deselectOnBackgroundClick = false;
                    EventSystem.current.SetSelectedGameObject(endGamePanel.GetComponentInChildren<Button>().gameObject);
                }
            }
        }
        if (showResult && EventSystem.current != null && EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(endGamePanel.GetComponentInChildren<Button>().gameObject);
    }
}
