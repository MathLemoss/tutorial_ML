using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class MainMenuNavigation : MonoBehaviour
{
    public GameObject homePage;
    public GameObject historyPage;
    public GameObject instructionsPage;
    public Button playButton;
    public Button historyButton;
    public Button instructionsButton;
    public Button historyBackButton;
    public Button instructionsBackButton;

    private Button returnSelection;
    private Button pageSelection;

    void Start()
    {
        if (EventSystem.current != null)
        {
            var input = EventSystem.current.GetComponent<InputSystemUIInputModule>();
            if (input != null)
                input.deselectOnBackgroundClick = false;
        }
        returnSelection = playButton;
        ShowHome();
    }

    public void ShowHistory()
    {
        returnSelection = historyButton;
        ShowPage(historyPage, historyBackButton);
    }

    public void ShowInstructions()
    {
        returnSelection = instructionsButton;
        ShowPage(instructionsPage, instructionsBackButton);
    }

    public void ShowHome()
    {
        ShowPage(homePage, returnSelection != null ? returnSelection : playButton);
    }

    private void ShowPage(GameObject page, Button selection)
    {
        homePage.SetActive(page == homePage);
        historyPage.SetActive(page == historyPage);
        instructionsPage.SetActive(page == instructionsPage);
        pageSelection = selection;
        SelectButton();
    }

    private void SelectButton()
    {
        if (EventSystem.current == null || pageSelection == null)
            return;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pageSelection.gameObject);
    }

    void Update()
    {
        bool back = (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            || (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame);
        if (back && !homePage.activeSelf)
        {
            ShowHome();
            return;
        }

        // Recupera o foco caso um clique ou uma reconexao do controle o tenha removido.
        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == null)
            SelectButton();
    }
}
