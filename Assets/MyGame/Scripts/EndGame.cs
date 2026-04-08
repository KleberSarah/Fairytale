using UnityEngine;

public class EndGame : MonoBehaviour
{    // Start is called once before the first execution of Update after the MonoBehaviour is created void Start
    public void QuitGame()
    {
        Application.Quit();
    }

    public void Menu()
    {
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
