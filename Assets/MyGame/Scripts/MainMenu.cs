using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public int mapSceneIndex = 0;

    public void OnStartGameButton()
    {
        // Alles auf Anfang setzen (1)
        PlayerPrefs.SetInt("LevelProgress", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene(mapSceneIndex);
    }
}