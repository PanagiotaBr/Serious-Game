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

    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private TextMeshProUGUI option1Text;
    [SerializeField] private TextMeshProUGUI option2Text;
    [SerializeField] private TextMeshProUGUI option3Text;

    Dialogue dialogue;
    int currentLine = 0;
    bool isTyping;

    public event Action OnShowDialogue;
    public event Action OnCloseDialogue;
    public static DialogueManager Instance{get; private set;}
    private void Awake()
    {
        Instance = this;
    }

    private bool awaitingChoice = false;

    public IEnumerator ShowDialogue(Dialogue newdialogue)
    {
        yield return new WaitForEndOfFrame();

        dialogue = newdialogue;

        OnShowDialogue?.Invoke();

        dialogueBox.SetActive(true);

        nameText.text = "";
        profileImage.sprite = dialogue.NPCPortrait;
        profileImage.gameObject.SetActive(dialogue.NPCPortrait != null);
        
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

    public IEnumerator TypeDialogue(DialogueLine dialogueline)
    {
        isTyping = true;
        dialogueText.text = "";

        // Set name and portrait based on speaker
        if (dialogueline.isPlayerSpeaking)
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

        // Type the dialogue letter by letter
        foreach (char letter in dialogueline.line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        // Show options if any
        if (dialogueline.hasOptions)
        {
            awaitingChoice = true;
            StartCoroutine(ShowOptions(dialogueline.options));
        }
        else
        {
            optionsPanel.SetActive(false);
        }
        isTyping = false;
        //yield return null;
    }  
    private IEnumerator TypeOptionText(TextMeshProUGUI textComponent, string fullText)
    {
        foreach (char letter in fullText.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
    }
    private IEnumerator ShowOptions(List<DialogueOption> options)
    {
        optionsPanel.SetActive(true);

        option1Text.text = "";
        option2Text.text = "";
        option3Text.text = "";
        // Set text for options, assuming there are up to 3 options for simplicity
        if (options.Count > 0)
            yield return StartCoroutine(TypeOptionText(option1Text, options[0].optionText));
        if (options.Count > 1)
            yield return StartCoroutine(TypeOptionText(option2Text, options[1].optionText));
        if (options.Count > 2)
            yield return StartCoroutine(TypeOptionText(option3Text, options[2].optionText));

        // Allow player to choose an option
        // Here you can use key presses, for example, to choose between options
    }

    public void HandleChoice(int choiceIndex)
    {
        var line = dialogue.Lines[currentLine];

        if (!line.hasOptions || choiceIndex >= line.options.Count)
            return;

        DialogueOption chosen = line.options[choiceIndex];

        awaitingChoice = false;
        optionsPanel.SetActive(false);

        // Show the response immediately
        DialogueLine responseLine = new DialogueLine
        {
            line = chosen.responseLine,
            isPlayerSpeaking = false,
            hasOptions = false
        };

        StartCoroutine(TypeDialogue(responseLine));

        TriggerAction(chosen.actionToTrigger);
    }

    private void TriggerAction(string action)
    {
        // Here you can handle specific actions based on the player's choice.
        if (action == "GoToIslandAbove")
        {
            // Trigger moving to the island above (implement your logic here)
            Debug.Log("Go to island above.");
        }
        else if (action == "GoToLeftIsland")
        {
            // Trigger moving to the left island
            Debug.Log("Go to left island.");
        }
        else if (action == "GoToRightIsland")
        {
            // Trigger moving to the right island
            Debug.Log("Go to right island.");
        }
    }
    private void ChoiceUpdate()
    {
        if (!dialogueBox.activeInHierarchy) return; // Only check for input if dialogue is showing

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            HandleChoice(0); // First option
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            HandleChoice(1); // Second option
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            HandleChoice(2); // Third option
        }
    }
    private void Update()
    {
        if (awaitingChoice)
            ChoiceUpdate();
    }
}
