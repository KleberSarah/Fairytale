using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinisher : MonoBehaviour
{
    // Dieser Wert wird jetzt primär aus den PlayerPrefs gelesen (dynamisch)
    public int unlockProgress;

    // Der Index deiner Map-Szene (siehe Build Settings)
    public int mapSceneIndex = 0;

    public void CompleteLevel()
    {
        // 1. Hole den Wert, den wir freischalten SOLLEN (wurde vom PointManager übergeben)
        // Fallback ist 'unlockProgress' aus dem Inspector, falls nichts gespeichert war
        int progressToUnlock = PlayerPrefs.GetInt("PotentialUnlock", unlockProgress);

        // 2. Hole den aktuellen Fortschritt des Spielers
        int currentProgress = PlayerPrefs.GetInt("LevelProgress", 1);

        // 3. Prüfen und Speichern
        if (progressToUnlock > currentProgress)
        {
            PlayerPrefs.SetInt("LevelProgress", progressToUnlock);
            PlayerPrefs.Save();
            Debug.Log("Neuer Fortschritt gespeichert: " + progressToUnlock);
        }
        else
        {
            Debug.Log("Fortschritt nicht erhöht (aktuell: " + currentProgress + ", neu: " + progressToUnlock + ")");
        }

        // 4. Zurück zur Map
        SceneManager.LoadScene(mapSceneIndex);
    }
}