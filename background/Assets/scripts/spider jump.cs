using UnityEngine;

public class SpiderJumpOnce : MonoBehaviour
{
    private Animator animator;
    private bool hasJumped = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasJumped && other.CompareTag("Player"))
        {
            animator.SetTrigger("jump trigger");
            hasJumped = true;
        }
    }
}
