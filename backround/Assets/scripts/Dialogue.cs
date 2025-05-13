using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue 
{   
    [SerializeField] private string playerName;
    [SerializeField] private string npcName;
    [SerializeField] private Sprite playerportrait;
    [SerializeField] private Sprite npcportrait;
    [SerializeField] List<DialogueLine> lines;

    public string PlayerName => playerName;
    public string NPCName => npcName;
    public Sprite PlayerPortrait => playerportrait;
    public Sprite NPCPortrait => npcportrait;
    public List<DialogueLine> Lines => lines;
}
[System.Serializable]
public class DialogueLine
{
    public bool isPlayerSpeaking; // true = player, false = NPC
    [TextArea]
    public string line;
}
