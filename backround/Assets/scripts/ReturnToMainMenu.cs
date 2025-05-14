using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenuTrigger : MonoBehaviour
{
    public int menuSceneIndex = 0; // 0 = index της σκηνής μενού

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(menuSceneIndex);
        }
    }
}
