using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public int mapSceneIndex = 0;

    public void OnStartGameButton()
    {
        // Level Fortschritt zurücksetzen
        PlayerPrefs.SetInt("LevelProgress", 1);
        
        // SICHERHEIT: Den Score beim Start eines GANZ NEUEN Spiels auf 0 setzen
        PlayerPrefs.SetInt("Score", 0); 
        
        PlayerPrefs.Save();

        SceneManager.LoadScene(mapSceneIndex);
    }
}