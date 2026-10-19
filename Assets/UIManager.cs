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
        hudText.text = $"Pontos: {GameController.Score}    Notas: {GameController.Collected}/{GameController.TotalCollectables}"
            + $"\nTempo: {GameController.ElapsedTime:0.0}s    Percurso: {Mathf.FloorToInt(level.Progress * 100f)}%";
        instructionsText.text = GameController.State == GameController.MatchState.Ready
            ? "ESPACO / A para comecar\nCada toque alterna entre subir e descer. Evite as paredes!"
            : "ESPACO / A: trocar direcao";
        instructionsText.gameObject.SetActive(!GameController.gameOver);

        if (endGamePanel.activeSelf != GameController.gameOver)
        {
            endGamePanel.SetActive(GameController.gameOver);
            if (GameController.gameOver)
            {
                string title = GameController.State == GameController.MatchState.Won
                    ? "Percurso concluido!" : "Marrone te pegou!";
                resultText.text = $"{title}\n<size=65%>Tempo: {GameController.ElapsedTime:0.0}s | Pontos: {GameController.Score}</size>";
                if (EventSystem.current != null)
                    EventSystem.current.SetSelectedGameObject(endGamePanel.GetComponentInChildren<Button>().gameObject);
            }
        }
    }
}
