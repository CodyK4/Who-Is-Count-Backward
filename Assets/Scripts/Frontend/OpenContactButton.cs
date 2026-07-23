using UnityEngine;

public class OpenContactButton : MonoBehaviour
{
    // Should be attached to contact buttons in the messages screen, to open the relevant conversation- ensure to fill in inspector values before use.

    [SerializeField] private Character contact;
    [SerializeField] private ConversationManager conversationManager;
    [SerializeField] private PhoneNavigation phoneNavigation;

    public void OpenContact()
    {
        phoneNavigation.GoToConversation();
        conversationManager.OpenConversation(contact);
    }
}
