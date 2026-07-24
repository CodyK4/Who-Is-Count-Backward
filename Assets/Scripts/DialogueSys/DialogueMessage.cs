using UnityEngine;
using System;

[Serializable]
public class DialogueMessage 
{
    public MessageSender sender = MessageSender.Contact;

    [TextArea]
    public string text;

    [Min(0f)]
    public float delay = 0.5f; // this is the delay between the message being sent and the message being displayed, in seconds, ideally accompanied by a typing animation
}

public enum MessageSender
{
    Player,
    Contact,
    System
}
