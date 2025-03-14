using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Load the MainGame scene
    public void PlayGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    // Quit the game (Only works in build mode)
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed!"); // Only visible in the editor
    }
}
