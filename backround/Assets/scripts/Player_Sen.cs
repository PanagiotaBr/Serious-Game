using UnityEngine;

public class Player_Sen : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D body;
    private Animator anim;
    private Vector3 respawnPoint;

    private void Awake() 
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        respawnPoint = transform.position;
    }

    private void Update()
    {
        body.linearVelocity = new Vector2(speed, body.linearVelocity.y);
        transform.localScale = Vector3.one;
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
    	if (collision.gameObject.CompareTag("Respawn"))
        {
            transform.position = respawnPoint;
        }
    }

}
