using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ConversationManager : MonoBehaviour
    // This class acts as the spine of the logic for the game. Handling conversation between the player and characters, displaying questions, scheduled messages, investigation state, etc.
{
    [Header("Contact Header")]
    [SerializeField] private TMP_Text contactNameText;
    [SerializeField] private Image profileImage;

    [Header("Messages")]
    [SerializeField] private Transform messageContainer;
    [SerializeField] private ChatMessageBubble playerBubblePrefab;
    [SerializeField] private ChatMessageBubble contactBubblePrefab;
    [SerializeField] private ScrollRect messageScrollRect;

    [Header("Scheduled Messages")]
    [SerializeField] private TimeRunner timeRunner;
    [SerializeField] private Character[] contacts; // Array of all contacts in the game, used to schedule generic messages based on time and investigation state

    [Header("Question UI")]
    [SerializeField] private Transform questionContainer;
    [SerializeField] private Button questionButtonPrefab;

    [Header("Typing Indicator")]
    [SerializeField] private GameObject typingIndicator;

    private Character currentContact;
    private bool responsePlaying;

    private readonly Dictionary<string, List<StoredMessage>> conversationHistory = new();

    //subscribe to the time runner's OnMinuteChanged event to check for scheduled messages every minute
    private void OnEnable()
    {
        if (timeRunner != null)
            timeRunner.OnMinuteChanged += CheckScheduledMessages;
    }

    private void OnDisable()
    {
        if (timeRunner != null)
            timeRunner.OnMinuteChanged -= CheckScheduledMessages;
    }
    //

    public void OpenConversation(Character contact)
    //Opens a conversation with the specified contact, updating the UI.
    {
        currentContact = contact;

        typingIndicator.SetActive(false);

        contactNameText.text = contact.displayName;
        profileImage.sprite = contact.profilePicture;

        DisplayConversationHistory();
        RefreshQuestions();
    }

    private List<StoredMessage> GetHistory(Character contact)
    //Gets the stored history of messages for the specified contact, creating a new list if one does not exist.
    {
        if (!conversationHistory.TryGetValue(contact.characterId, out List<StoredMessage> history))
        {
            history = new List<StoredMessage>();

            conversationHistory.Add(contact.characterId, history);
        }

        return history;
    }

    private void DisplayConversationHistory()
    //Displays the stored conversation history.
    {
        foreach (Transform child in messageContainer)
            Destroy(child.gameObject);

        foreach (StoredMessage message in GetHistory(currentContact))
            CreateMessageBubble(message.text, message.isPlayer, message.hour, message.minute);
    }

    private void RefreshQuestions()
    //Refreshes the question buttons based on the current contact and the investigation state
    {
        foreach (Transform child in questionContainer)
        {
            Destroy(child.gameObject);
        }

        if (currentContact == null || responsePlaying)
            return;

        foreach (DialogueQuestion question in currentContact.questions)
        {
            if (!CanAskQuestion(question))
                continue;

            Button button = Instantiate(questionButtonPrefab, questionContainer);

            button.GetComponentInChildren<TMP_Text>().text = question.questionText;

            DialogueQuestion _question = question;

            button.onClick.AddListener(() => StartCoroutine(AskQuestion(_question)));
        }
    }

    private bool CanAskQuestion(DialogueQuestion question)
    //Checks if the question can be asked, based on if the question has been asked and if the required clues have been discovered
    {
        InvestigationState state = InvestigationState.Instance;

        if (question.askOnce && state.HasAsked(currentContact.characterId, question.questionId))
        {
            return false;
        }

        foreach (string requiredClue in question.requiredClues)
        {
            if (!state.HasClue(requiredClue))
            {
                return false;
            }
        }

        return true;
    }

    private IEnumerator AskQuestion(DialogueQuestion question)
    //Handles process of asking a question, marking it as asked, and displaying the response messages
    {
        if (responsePlaying)
            yield break;

        responsePlaying = true;

        Character respondingContact = currentContact;

        InvestigationState.Instance.MarkAsked(respondingContact.characterId, question.questionId);

        RefreshQuestions();

        AddMessage(respondingContact, question.questionText, true);

        foreach (DialogueMessage message in question.response)
        {
            if (message.delay > 0f)
            {
                if (currentContact == respondingContact)
                {
                    typingIndicator.SetActive(message.sender == MessageSender.Contact);
                }
            }

            yield return new WaitForSeconds(message.delay);

            if (currentContact == respondingContact)
            {
                typingIndicator.SetActive(false);
            }

            AddMessage(respondingContact, message.text, message.sender == MessageSender.Player);

        }

        foreach (string clue in question.grantedClues)
            InvestigationState.Instance.AddClue(clue);

        responsePlaying = false;

        if (currentContact == respondingContact)
            RefreshQuestions();
    }

    private void AddMessage(Character contact, string text, bool isPlayer)
    //Adds a message to the chat UI, instantiating the appropriate bubble prefab based on the sender
    {
        int hour = timeRunner.CurrentHour;
        int minute = timeRunner.CurrentMinute;

        Debug.Log($"Message added to {contact.displayName}: {text}");

        GetHistory(contact).Add(new StoredMessage(text, isPlayer, hour, minute));

        if (currentContact == contact)
        {
            CreateMessageBubble(text, isPlayer, hour, minute);
        }
    }

    private void CreateMessageBubble(string text, bool isPlayer, int hour, int minute)
    {
        ChatMessageBubble bubblePrefab = isPlayer ? playerBubblePrefab : contactBubblePrefab;

        ChatMessageBubble bubble = Instantiate(bubblePrefab, messageContainer);

        bubble.SetMessage(text, hour, minute);

        Canvas.ForceUpdateCanvases();
        messageScrollRect.verticalNormalizedPosition = 0f; 
    }

    private void CheckScheduledMessages(int hour, int minute)
        //Checks all contacts for scheduled messages that are to be triggered at the specific hour and minute, and if conditions are met.
    {
        Debug.Log($"Checking messages at {hour:00}:{minute:00}");

        foreach (Character contact in contacts)
        {
            foreach (ScheduledMessage scheduledMessage in contact.scheduledMessages)
            {
                Debug.Log($"Checking {contact.displayName} message " + $"{scheduledMessage.messageId}");

                if (!CanTriggerScheduledMessage(contact, scheduledMessage, hour, minute))
                    continue;

                Debug.Log($"Triggered message from {contact.displayName}");

                InvestigationState.Instance.MarkMessageTriggered(contact.characterId, scheduledMessage.messageId);

                StartCoroutine(PlayScheduledMessage(contact, scheduledMessage));
            }
        }
    }

    private bool CanTriggerScheduledMessage(Character contact, ScheduledMessage scheduledMessage, int hour, int minute)
    //Checks if a scheduled message can be triggered based on the current time and investigation state
    {
        InvestigationState state = InvestigationState.Instance;

        if (state.HasTriggeredMessage(contact.characterId, scheduledMessage.messageId))
        {
            return false;
        }

        bool timeReached =
            hour > scheduledMessage.triggerHour ||
            hour == scheduledMessage.triggerHour &&
            minute >= scheduledMessage.triggerMinute; // >= incase of loading errors, etc.

        if (!timeReached)
        {
            return false;
        }

        foreach (string requiredClue in scheduledMessage.requiredClues)
        //if the player has not discovered a required clue, the message cannot be triggered
        {
            if (!state.HasClue(requiredClue))
                return false;
        }

        return true;
    }

    private IEnumerator PlayScheduledMessage(Character contact, ScheduledMessage scheduledMessage)
        //Similar to AskQuestion, this IEnumerator handles the process of displaying a scheduled message.
    {
        foreach (DialogueMessage message in scheduledMessage.messages)
        {
            if (currentContact == contact)
            {
                typingIndicator.SetActive(true);
            }

            if (message.delay > 0f)
            {
                yield return new WaitForSeconds(message.delay);
            }

            if (currentContact == contact)
            {
                typingIndicator.SetActive(false);
            }

            AddMessage(contact, message.text, false);
        }

        foreach(string clue in scheduledMessage.grantedClues)
        {
            InvestigationState.Instance.AddClue(clue);
        }

        RefreshQuestions();
    }
}
