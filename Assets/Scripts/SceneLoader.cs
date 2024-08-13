using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Tooltip("Enter the build index of the scene you want to load.")]
    public int sceneIndex;

    // This method can be linked to the button's OnClick event
    public void LoadScene()
    {
        // Check if the scene index is within valid range
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log($"Loading scene at index {sceneIndex}.");
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogWarning($"Invalid scene index: {sceneIndex}. Ensure it is within the range of scenes in Build Settings.");
        }
    }
}
