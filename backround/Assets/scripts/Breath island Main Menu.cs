using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public int menuSceneIndex = 0;
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(menuSceneIndex);  
    }
}
