using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonReturnToSampleScene : MonoBehaviour
{
    [SerializeField] private Button returnButton;

    private void Start()
    {
        if (returnButton != null)
        {
            returnButton.onClick.AddListener(LoadSampleScene);
        }
        else
        {
            Debug.LogWarning("Return button not assigned in inspector.");
        }
    }

    private void LoadSampleScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}

