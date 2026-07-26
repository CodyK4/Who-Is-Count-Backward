using UnityEngine;

public class PhoneNavigation : MonoBehaviour
{
    //A very simple script for rudamentary phone navigation 

    [SerializeField] private GameObject messagesScreen;
    [SerializeField] private GameObject conversationsScreen;

    public void Start()
    {
        messagesScreen.SetActive(true);
        conversationsScreen.SetActive(false);
    }

    public void GoToMessages()
    {
        messagesScreen.SetActive(true);
        conversationsScreen.SetActive(false);
    }

    public void GoToConversation()
    {
        messagesScreen.SetActive(false);
        conversationsScreen.SetActive(true);
    }

    public void HideAllScreens()
    {
        messagesScreen.SetActive(false);
        conversationsScreen.SetActive(false);
    }
}
