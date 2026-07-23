using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScheduledMessage
{
    //this defines a generic text that the contact will send, not tied to any questions

    public string messageId; //easier in scripting

    //schedule when to send messages to player
    [Range(0, 23)]
    public int triggerHour;
    [Range(0, 59)]
    public int triggerMinute;

    [Tooltip("Optional- require clues before message can arrive")]
    public List<string> requiredClues;

    [Tooltip("Optional- Grant clues to the player to later ask questions")]
    public List<string> grantedClues;

    public List<DialogueMessage> messages;

}
