using UnityEngine;

public class Player_move_platform : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jump_speed;
    private Rigidbody2D body;
    private Animator anim;
    private bool grounded;

    private Vector3 respawnPoint;
    public GameObject fallDetector;

    private void Awake() 
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        respawnPoint = transform.position;
    }

    private void Update()
    {
        float HorizontalInput = Input.GetAxis("Horizontal");
        body.linearVelocity  = new Vector2(HorizontalInput * speed, body.linearVelocity.y);

        if (HorizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (HorizontalInput < -0.01f)
            transform.localScale = new Vector3(-1,1,1);

        if(Input.GetButtonDown("Jump") && grounded)
            Jump();

        anim.SetBool("run", HorizontalInput != 0);
        anim.SetBool("grounded", grounded);
        fallDetector.transform.position = new Vector2(transform.position.x,fallDetector.transform.position.y);

    }

    private void Jump() 
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, jump_speed);
        anim.SetTrigger("jump");
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // Check if contact point is below the player
                if (contact.point.y < transform.position.y - 0.1f)
                {
                    grounded = true;
                    return; // Exit after finding a valid ground contact
                }
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
    	if (collision.gameObject.CompareTag("Respawn"))
        {
            transform.position = respawnPoint;
        }
    }

}
