using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject dialogueBox;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] Image profileImage;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] int lettersPerSecond;

    public event Action OnShowDialogue;
    public event Action OnCloseDialogue;
    public static DialogueManager Instance{get; private set;}
    private void Awake()
    {
        Instance = this;
    }

    Dialogue dialogue;
    int currentLine = 0;
    bool isTyping;
    public IEnumerator ShowDialogue(Dialogue newdialogue)
    {
        yield return new WaitForEndOfFrame();

        dialogue = newdialogue;

        OnShowDialogue?.Invoke();

        dialogueBox.SetActive(true);

        nameText.text = "";
        profileImage.sprite = null;
        profileImage.gameObject.SetActive(false);

        currentLine = 0;
        StartCoroutine(TypeDialogue(dialogue.Lines[currentLine]));
    }
    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isTyping)
        {
            ++currentLine;
            if (currentLine < dialogue.Lines.Count)
            {
                StartCoroutine(TypeDialogue(dialogue.Lines[currentLine]));
            }
            else
            {
                currentLine = 0;
                dialogueBox.SetActive(false);
                nameText.text = "";
                profileImage.sprite = null;
                profileImage.gameObject.SetActive(false);
                OnCloseDialogue?.Invoke();
            }
        }
    }

    public IEnumerator TypeDialogue(DialogueLine line)
    {
        isTyping = true;
        dialogueText.text = "";

        if (line.isPlayerSpeaking)
        {
            nameText.text = dialogue.PlayerName;
            profileImage.sprite = dialogue.PlayerPortrait;
        }
        else
        {
            nameText.text = dialogue.NPCName;
            profileImage.sprite = dialogue.NPCPortrait;
        }

        profileImage.gameObject.SetActive(profileImage.sprite != null);

        foreach (var letter in line.line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond); // Wait for the next frame
        }
        isTyping = false;
    }
}
