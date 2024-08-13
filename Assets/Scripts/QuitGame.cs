using UnityEngine;

public class QuitGame : MonoBehaviour
{
    // This method can be linked to the button's OnClick event
    public void Quit()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();

        // This line will only run in the editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
