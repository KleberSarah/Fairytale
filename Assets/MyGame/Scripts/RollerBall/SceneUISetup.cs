using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneUISetup : MonoBehaviour
{
    [Header("Hier die UI aus DIESER Szene reinziehen")]
    public TMP_Text myTimerText;
    public Slider myPointSlider;

    void Start()
    {
        // Wir suchen den PointManager (der ist ja DontDestroyOnLoad)
        if (PointManager.Instance != null)
        {
            // Und drücken ihm unsere UI in die Hand
            PointManager.Instance.SetUI(myTimerText, myPointSlider);
            Debug.Log("UI erfolgreich an PointManager übergeben.");
        }
        else
        {
            // Fallback: Falls man die Szene einzeln testet ohne PointManager
            Debug.LogWarning("Kein PointManager gefunden! (Startest du nicht vom Menü/Map?)");
        }
    }
}