using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SuspectButton : MonoBehaviour
{
    [SerializeField] private Character contact;
    [SerializeField] private AccusationManager accusationManager;
    [SerializeField] private TMP_Text buttonText;

    private void Awake()
    {
        if (buttonText != null && contact != null)
        {
            buttonText.text = contact.displayName;
        }

        GetComponent<Button>().onClick.AddListener(SelectThisContact);
    }

    private void SelectThisContact()
    {
        accusationManager.SelectSuspect(contact);
    }
}
