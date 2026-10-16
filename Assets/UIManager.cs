using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject endGamePanel;

    void Awake()
    {
        GameController.Init(GameObject.FindGameObjectsWithTag("Coletavel").Length);
        endGamePanel.SetActive(false);
    }

    void Update()
    {
        if (endGamePanel.activeSelf != GameController.gameOver)
        {
            endGamePanel.SetActive(GameController.gameOver);
        }
    }
}
