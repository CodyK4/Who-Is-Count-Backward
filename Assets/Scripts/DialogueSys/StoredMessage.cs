using System;
using UnityEngine;

[Serializable]
public class StoredMessage
//This class is used to store messages in the dialogue system, it contains the text of a message, and if it was sent by the player or not.
{
    public string text;
    public bool isPlayer;
    public int hour;
    public int minute;

    public StoredMessage(string text, bool isPlayer, int hour, int minute)
    {
        this.text = text;
        this.isPlayer = isPlayer;
        this.hour = hour;
        this.minute = minute;
    }
}
