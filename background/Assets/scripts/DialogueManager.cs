using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image profileImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private int lettersPerSecond = 30;

    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private TextMeshProUGUI option1Text;
    [SerializeField] private TextMeshProUGUI option2Text;
    [SerializeField] private TextMeshProUGUI option3Text;

    [Header("Mobile UI Buttons")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Button option1Button;
    [SerializeField] private Button option2Button;
    [SerializeField] private Button option3Button;

    private Dialogue dialogue;
    private int currentLine = 0;
    private bool isTyping = false;
    private bool awaitingChoice = false;

    private int currentAction = 0;
    private int totalOptions = 0;

    public event Action OnShowDialogue;
    public event Action OnCloseDialogue;
    public static DialogueManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        if (dialogueBox == null) Debug.LogError("Dialogue Box is not assigned!");
        if (dialogueText == null) Debug.LogError("Dialogue Text is not assigned!");
        if (profileImage == null) Debug.LogError("Profile Image is not assigned!");
        if (nameText == null) Debug.LogError("Name Text is not assigned!");
    }

    private void Start()
    {
        if (nextButton != null)
            nextButton.onClick.AddListener(() => OnNextClicked());

        if (option1Button != null)
            option1Button.onClick.AddListener(() => HandleChoice(0));
        if (option2Button != null)
            option2Button.onClick.AddListener(() => HandleChoice(1));
        if (option3Button != null)
            option3Button.onClick.AddListener(() => HandleChoice(2));
    }

    public IEnumerator ShowDialogue(Dialogue newDialogue)
    {
        yield return new WaitForEndOfFrame();

        dialogue = newDialogue;

        OnShowDialogue?.Invoke();
        dialogueBox.SetActive(true);
        nextButton.gameObject.SetActive(true);
        nameText.text = "";
        profileImage.sprite = dialogue.NPCPortrait;
        profileImage.gameObject.SetActive(dialogue.NPCPortrait != null);

        currentLine = 0;
        StartCoroutine(TypeDialogue(dialogue.Lines[currentLine]));
    }

    private void OnNextClicked()
    {
        if (isTyping || awaitingChoice)
            return;

        ++currentLine;
        if (currentLine < dialogue.Lines.Count)
        {
            StartCoroutine(TypeDialogue(dialogue.Lines[currentLine]));
        }
        else
        {
            currentLine = 0;
            dialogueBox.SetActive(false);
            nextButton.gameObject.SetActive(false);
            nameText.text = "";
            profileImage.sprite = null;
            profileImage.gameObject.SetActive(false);
            OnCloseDialogue?.Invoke();
        }
    }

    public void HandleUpdate()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.Space) && !isTyping && !awaitingChoice)
        {
            OnNextClicked();
        }
#endif
    }

    public IEnumerator TypeDialogue(DialogueLine dialogueline)
    {
        isTyping = true;
        dialogueText.text = "";

        // Set name and portrait
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

        // Type effect
        foreach (char letter in dialogueline.line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }

        if (dialogueline.hasOptions)
        {
            awaitingChoice = true;
            currentAction = 0;
            yield return StartCoroutine(ShowOptions(dialogueline.options));
        }
        else
        {
            optionsPanel.SetActive(false);
            
        }

        isTyping = false;
    }

    private IEnumerator TypeOptionText(TextMeshProUGUI textComponent, string fullText)
    {
        textComponent.text = "";
        foreach (char letter in fullText.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
    }

    private IEnumerator ShowOptions(List<DialogueOption> options)
    {
        optionsPanel.SetActive(true);

        // Hide and clear all buttons/texts first
        option1Button.gameObject.SetActive(false);
        option2Button.gameObject.SetActive(false);
        option3Button.gameObject.SetActive(false);

        option1Text.text = "";
        option2Text.text = "";
        option3Text.text = "";

        totalOptions = options.Count;

        // Show and animate each option one after the other
        if (options.Count > 0)
        {
            option1Button.gameObject.SetActive(true);
            yield return StartCoroutine(TypeOptionText(option1Text, options[0].optionText));
            yield return new WaitForSeconds(0.2f);
        }

        if (options.Count > 1)
        {
            option2Button.gameObject.SetActive(true);
            yield return StartCoroutine(TypeOptionText(option2Text, options[1].optionText));
            yield return new WaitForSeconds(0.2f);
        }

        if (options.Count > 2)
        {
            option3Button.gameObject.SetActive(true);
            yield return StartCoroutine(TypeOptionText(option3Text, options[2].optionText));
        }

        UpdateActionSelection(currentAction);
    }


    private void SetOptionButtonsActive(bool active)
    {
        if (option1Button != null) option1Button.gameObject.SetActive(active);
        if (option2Button != null) option2Button.gameObject.SetActive(active);
        if (option3Button != null) option3Button.gameObject.SetActive(active);
    }

    public void HandleChoice(int choiceIndex)
    {
        var line = dialogue.Lines[currentLine];

        if (!line.hasOptions || choiceIndex >= line.options.Count)
            return;

        DialogueOption chosen = line.options[choiceIndex];
        awaitingChoice = false;

        optionsPanel.SetActive(false);
        SetOptionButtonsActive(false);

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
        if (action == "GoToIslandAbove")
        {
            Debug.Log("Go to island above.");
        }
        else if (action == "GoToLeftIsland")
        {
            Debug.Log("Go to left island.");
        }
        else if (action == "GoToRightIsland")
        {
            Debug.Log("Go to right island.");
        }
    }

    private void HandleActionSelection()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.DownArrow) && currentAction < totalOptions - 1)
            ++currentAction;
        else if (Input.GetKeyDown(KeyCode.UpArrow) && currentAction > 0)
            --currentAction;

        UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Space) && !isTyping)
        {
            HandleChoice(currentAction);
        }
#endif
    }

    private void UpdateActionSelection(int selectedIndex)
    {
        option1Text.color = Color.black;
        option2Text.color = Color.black;
        option3Text.color = Color.black;

        // if (selectedIndex == 0)
        //     option1Text.color = Color.blue;
        // else if (selectedIndex == 1)
        //     option2Text.color = Color.blue;
        // else if (selectedIndex == 2)
        //     option3Text.color = Color.blue;
    }

    private void Update()
    {
        if (awaitingChoice)
        {
            HandleActionSelection();
        }
        else
        {
            HandleUpdate();
        }
    }
}
