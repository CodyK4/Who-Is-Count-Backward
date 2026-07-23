using UnityEngine;
using TMPro;

public class ChatMessageBubble : MonoBehaviour
    // A simple script to set the message text of a chat message bubble in the UI.
{
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private TMP_Text timeStampText;

    public void SetMessage(string message, int hour, int minute)
    {
        messageText.text = message;
        timeStampText.text = $"{hour:00}:{minute:00}";
    }
}
