using UnityEngine;

public class Player_Sen : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D body;
    private Animator anim;

    private void Awake() 
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        body.linearVelocity = new Vector2(speed, body.linearVelocity.y);
        transform.localScale = Vector3.one;
    }

}
