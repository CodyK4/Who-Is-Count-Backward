using System.Collections.Generic;
using UnityEngine;

public class InvestigationState : MonoBehaviour
{
    public static InvestigationState Instance { get; private set; }

    private readonly HashSet<string> discoveredClues = new();
    private readonly HashSet<string> askedQuestions = new();
    private readonly HashSet<string> triggeredMessages = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public bool HasClue(string clueId)
    // Check if the player has discovered the clue
    {
        return discoveredClues.Contains(clueId);
    }

    public void AddClue(string clueId)
    // Adds a clue to the player's discovered clues
    {
        if (!string.IsNullOrWhiteSpace(clueId))
        {
            discoveredClues.Add(clueId);
        }
    }

    public bool HasAsked(string characterId, string questionId)
    // Check if the player has already asked a specific question to a character
    {
        return askedQuestions.Contains($"{characterId}:{questionId}");
    }

    public void MarkAsked(string characterId, string questionId)
    // Marks a specific question as asked to a character
    {
        askedQuestions.Add($"{characterId}:{questionId}");
    }

    public bool HasTriggeredMessage(string characterId, string messageId)
    {
        return triggeredMessages.Contains($"{characterId}:{messageId}");
    }

    public void MarkMessageTriggered(string characterId, string messageId)
    {
        triggeredMessages.Add($"{characterId}:{messageId}");
    }
}
