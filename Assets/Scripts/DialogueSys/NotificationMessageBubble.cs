using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class NotificationMessageBubble : MonoBehaviour
// A simple script to set the message text of a chat message bubble in the UI.
{
    [Header("Animation")]
    [SerializeField] private CanvasGroup canvas;

    [Header("Text)")]
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private TMP_Text timeStampText;

    public void SetMessage(Character contact, int hour, int minute)
    {
        messageText.text = $"New message from {contact.displayName}";
        timeStampText.text = $"{hour:00}:{minute:00}";

    }

    public void PlayAnim()
    {
        canvas.alpha = 0f;

        Sequence seq = DOTween.Sequence();

        seq.Append(canvas.DOFade(1f, 0.25f));

        seq.AppendInterval(2f);

        seq.Append(canvas.DOFade(0f, 0.25f));

        seq.OnComplete(() => { Destroy(gameObject); });
    }
}
