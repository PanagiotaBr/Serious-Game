using UnityEngine;
using System.Collections;  // Needed for IEnumerator
using System.Collections.Generic;  // Needed for List

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public LayerMask solidObjectsLayer;
    public LayerMask groundLayer;
    public LayerMask waterLayer;
    public bool isMoving;
    private Vector2 input;
    private Animator animator;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);
                
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;
                if (IsWalkable(targetPos))
                    StartCoroutine(Move(targetPos));
            }
        }

        animator.SetBool("isMoving", isMoving);
    }
    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;
    }
    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapBox(targetPos, new Vector2(0.8f, 0.8f), 0f, solidObjectsLayer) != null)
        {
            Debug.Log("Blocked by solid object: " + targetPos);
            return false;
        }

        Vector2 checkSize = new Vector2(0.8f, 0.8f); 
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(targetPos, checkSize, 0f, groundLayer);

        foreach (Collider2D hitCollider in hitColliders)
        {

            if (hitCollider.CompareTag("Bridge"))
            {
                Debug.Log("It's a bridge — allow passage.");
                return true;
            }

            if (hitCollider != null)
            {
                Debug.Log("Hit: " + hitCollider.name);
                if (hitCollider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    return false;
                }
            }
        }

        return true;
    }



}