using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(
    fileName = "New Character",
    menuName = "Characters")]
public class Character : ScriptableObject
{
    public string characterId; //easier for scripting
    public string displayName; //display name in UI
    public Sprite profilePicture; //profile picture in UI
    public OpenContactButton contactButton;

    public List<ScheduledMessage> scheduledMessages; //list of generic texts that the character can say, not tied to any specific question or clue
    public List<DialogueQuestion> questions; //list of questions for this character

}
