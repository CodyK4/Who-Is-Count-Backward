using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AccusationManager : MonoBehaviour
{
    [Header("Accusation UI")]
    [SerializeField] private GameObject accusationPanel;
    [SerializeField] private TMP_Text warningText;
    [SerializeField] private Button confirmButton;

    [Header("Ending UI")]
    [SerializeField] private GameObject winEndingPanel;
    [SerializeField] private GameObject loseEndingPanel;
    [SerializeField] private TMP_Text loseEndingText;

    [Header("Script References")]
    [SerializeField] private TimeRunner timeRunner;
    [SerializeField] private PhoneNavigation phoneNavigation;

    [Header("Correct Answer")]
    [SerializeField] private Character correctContact; // the correct contact to accuse

    private Character selectedContact; // the contact currently selected by the player
    private bool endingStarted;

    private void OnEnable()
    {
        timeRunner.onTimeExpired += HandleTimeExpired;
    }

    private void OnDisable()
    {
        timeRunner.onTimeExpired -= HandleTimeExpired;
    }

    private void Start()
    // hide all panels at the start of the game.
    {
        accusationPanel.SetActive(false);
        winEndingPanel.SetActive(false);
        loseEndingPanel.SetActive(false);

        confirmButton.interactable = false;
    }

    public void OpenAccusationPanel()
    // opens the accusation panel, and resets the selected contact
    {
        if (endingStarted)
        {
            return;
        }

        selectedContact = null;

        warningText.text = "Select someone to accuse.";
        confirmButton.interactable = false;

        phoneNavigation.HideAllScreens();
        accusationPanel.SetActive(true);
    }
    
    public void ClearAccusationPanel()
    // clears the accusation panel, and resets the selected contact
    {
        if (endingStarted)
        {
            return;
        }

        selectedContact = null;
        accusationPanel.SetActive(false);

        phoneNavigation.GoToMessages();
    }

    public void SelectSuspect(Character contact)
    // selects a suspect from the accusation panel, updating text & enabling confirm button
    {
        if (endingStarted || contact == null)
        {
            return;
        }

        selectedContact = contact;

        warningText.text = $"Are you sure you want to accuse {contact.displayName}?";

        confirmButton.interactable = true;
    }

    public void ConfirmAccusation()
    // confirms the accusation, checks if the selected contact is correct, and shows the appropriate ending panel
    {
        if (endingStarted || selectedContact == null)
        {
            return;
        }

        endingStarted = true;

        accusationPanel.SetActive(false);
        phoneNavigation.HideAllScreens();

        bool accusationCorrect = selectedContact == correctContact;

        timeRunner.SkipToMidnight();

        if (accusationCorrect)
        {
            winEndingPanel.SetActive(true);
        }
        else
        {
            loseEndingPanel.SetActive(true);
            loseEndingText.text = "You are Incorrect.";
        }
    }
    
    private void HandleTimeExpired()
    {
        endingStarted = true;
        accusationPanel.SetActive(false);
        phoneNavigation.HideAllScreens();
        loseEndingPanel.SetActive(true);
        loseEndingText.text = "You have run out of time.";
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
