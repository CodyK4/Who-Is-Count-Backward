using TMPro;
using UnityEngine;

public class OpenContactButton : MonoBehaviour
{
    // Should be attached to contact buttons in the messages screen, to open the relevant conversation- ensure to fill in inspector values before use.

    [SerializeField] private Character contact;
    [SerializeField] private ConversationManager conversationManager;
    [SerializeField] private PhoneNavigation phoneNavigation;
    [SerializeField] private TMP_Text lastMessageText;

    public void Awake()
    {
        contact.contactButton = this;
    }

    public void SetLastMessage(string message, bool unread)
    {
        lastMessageText.text = message;
        lastMessageText.fontStyle = unread ? FontStyles.Bold : FontStyles.Normal;
    }

    public void MarkAsRead()
    {
        lastMessageText.fontStyle = FontStyles.Normal;
    }

    public void OpenContact()
    {
        phoneNavigation.GoToConversation();
        conversationManager.OpenConversation(contact);
    }
}
