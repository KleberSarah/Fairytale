using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinisher : MonoBehaviour
{
    // Dieser Wert wird jetzt prim�r aus den PlayerPrefs gelesen (dynamisch)
    public int unlockProgress;

    // Der Index deiner Map-Szene (siehe Build Settings)
    public int mapSceneIndex = 0;
    public int Levelindex; // Szene, die geladen wird, wenn der Spieler verliert

    public void CompleteLevel()
    {
        // 1. Hole den Wert, den wir freischalten SOLLEN (wurde vom PointManager �bergeben)
        // Fallback ist 'unlockProgress' aus dem Inspector, falls nichts gespeichert war
        int progressToUnlock = PlayerPrefs.GetInt("PotentialUnlock", unlockProgress);

        // 2. Hole den aktuellen Fortschritt des Spielers
        int currentProgress = PlayerPrefs.GetInt("LevelProgress", 1);

        // 3. Pr�fen und Speichern
        if (progressToUnlock > currentProgress)
        {
            PlayerPrefs.SetInt("LevelProgress", progressToUnlock);
            PlayerPrefs.Save();
            Debug.Log("Neuer Fortschritt gespeichert: " + progressToUnlock);
        }
        else
        {
            Debug.Log("Fortschritt nicht erh�ht (aktuell: " + currentProgress + ", neu: " + progressToUnlock + ")");
        }

        // 4. Zur�ck zur Map
        SceneManager.LoadScene(mapSceneIndex);
    }
    public void ReloadLevel()
    {
        SceneManager.LoadScene(Levelindex);
    }
}