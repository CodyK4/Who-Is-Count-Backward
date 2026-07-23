using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueQuestion 
{
    //this defines what questions can be asked of the character, if they require previous clues, etc.

    public string questionId; //easier in scripting

    [TextArea]
    public string questionText; //text that will be displayed in the dialogue box for the player to select

    public bool askOnce = true; // if true, the question will be removed from the list of available questions after it is asked once

    public List<string> requiredClues; //if the player does not have these clues, do not show the question as avaliable to ask
    public List<string> grantedClues; //if the player asks this question, they will be granted these clues

    public List<DialogueMessage> response; //the response that the character will give when asked this question
}
