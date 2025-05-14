using UnityEngine;
using TMPro;

public class SpiderInteraction : MonoBehaviour
{
    private Animator animator;
    private bool fixedOnce = false;

    [SerializeField] private TextMeshProUGUI spiderText;  // <-- αναφορά στο κείμενο

    void Start()
    {
        animator = GetComponent<Animator>();
        spiderText.text = "Please help me, I need a hand :(";
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!fixedOnce && other.CompareTag("Player"))
        {
            animator.SetTrigger("FixSpider");
            fixedOnce = true;

            spiderText.text = "Thank you! :)";
            Debug.Log("Spider fixed!");
        }
        
    }
}
