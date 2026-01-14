using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinisher : MonoBehaviour
{
    // Welcher Fortschritt wird freigeschaltet, wenn dieses Level beendet wird?
    // In Level 1 trägst du hier '2' ein.
    // In Level 2 trägst du hier '3' ein.
    public int unlockProgress = 2;

    // Der Index deiner Map-Szene (siehe Build Settings)
    public int mapSceneIndex = 0;

    // Diese Methode musst du aufrufen, wenn der Spieler das Ziel erreicht (z.B. durch einen Trigger oder Button)
    public void CompleteLevel()
    {
        // Hole den alten Fortschritt
        int currentProgress = PlayerPrefs.GetInt("LevelProgress", 1);

        // Wir speichern nur, wenn wir wirklich weiter sind als vorher 
        // (damit man nicht Level 1 spielt und plötzlich Level 3 verliert)
        if (unlockProgress > currentProgress)
        {
            PlayerPrefs.SetInt("LevelProgress", unlockProgress);
            PlayerPrefs.Save(); // Wichtig: Auf die Festplatte schreiben
            Debug.Log("Fortschritt gespeichert: " + unlockProgress);
        }

        // Zurück zur Map
        SceneManager.LoadScene(mapSceneIndex);
    }
}