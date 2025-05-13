using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialogue dialogue;
    [SerializeField] private Sprite UpSprite;
    [SerializeField] private Sprite DownSprite;
    [SerializeField] private Sprite LeftSprite;
    [SerializeField] private Sprite RightSprite;
    [SerializeField] private Sprite IdleSprite;

    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Interact()
    {
        FacePlayer();
        StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue));
    }
    void FacePlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player == null || spriteRenderer == null)
        {
            Debug.LogError("Missing player or sprite renderer.");
            return;
        }

        Vector3 direction = (player.transform.position - transform.position).normalized;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Face left or right
            spriteRenderer.sprite = direction.x > 0 ? RightSprite : LeftSprite;
        }
        else
        {
            // Face up or down
            spriteRenderer.sprite = direction.y > 0 ? UpSprite : DownSprite;
        }
    }
    private void OnEnable()
    {
        if (DialogueManager.Instance != null)
            DialogueManager.Instance.OnCloseDialogue += ResetToIdle;
    }

    private void OnDisable()
    {
        if (DialogueManager.Instance != null)
            DialogueManager.Instance.OnCloseDialogue -= ResetToIdle;
    }
    private void ResetToIdle()
    {
        if (spriteRenderer != null && IdleSprite != null)
        {
            spriteRenderer.sprite = IdleSprite;
        }
    }
}

