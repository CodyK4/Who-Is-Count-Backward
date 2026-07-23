using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "New Character",
    menuName = "Characters")]
public class Character : ScriptableObject
{
    public string characterId; //easier for scripting
    public string displayName; //display name in UI
    public Sprite profilePicture; //profile picture in UI

    public List<DialogueQuestion> questions; //list of questions for this character

}
