using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [SerializeField] private string playerName;
    [SerializeField] private string npcName;
    [SerializeField] private Sprite playerPortrait;
    [SerializeField] private Sprite npcPortrait;
    [SerializeField] private List<DialogueLine> lines;

    public string PlayerName => playerName;
    public string NPCName => npcName;
    public Sprite PlayerPortrait => playerPortrait;
    public Sprite NPCPortrait => npcPortrait;
    public List<DialogueLine> Lines => lines;
}

[System.Serializable]
public class DialogueLine
{
    [TextArea]
    public string line;
    public bool hasOptions;  // If true, the NPC will present options
    public List<DialogueOption> options;  // List of options to display
    public bool isPlayerSpeaking;
}

[System.Serializable]
public class DialogueOption
{
    public string optionText;
    public string actionToTrigger;  // Action to trigger for this specific choice
    public string responseLine;
}
