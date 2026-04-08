using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PointManager : MonoBehaviour
{
    [Header("UI Elemente")]
    public TMP_Text timeText;
    public Slider slider;

    [Header("Einstellungen")]
    public int bossSceneIndex; // Die Szene für den Kobold
    public float timeRemaining = 45f;
    
    private int points = 0;
    private bool isGameActive = true;

    void Start()
    {
        // UI initialisieren
        if (slider != null) 
        {
            slider.minValue = 0;
            slider.maxValue = 1;
            slider.value = 0;
        }
    }

    void Update()
    {
        if (!isGameActive) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            if (timeText != null) 
            {
                timeText.text = "Time: " + Mathf.CeilToInt(timeRemaining).ToString();
            }
        }
        else
        {
            GoToBossFight();
        }
    }

    public void AddPoints(int amount)
    {
        points += amount;
        if (slider != null) 
        {
            slider.value = points * 0.1f;
        }
    }

    public void GoToBossFight()
    {
        isGameActive = false;
        
        // TAFEL-LOGIK: Punkte für die nächste Szene in den PlayerPrefs speichern!
        PlayerPrefs.SetInt("Score", points);
        PlayerPrefs.Save();
        
        SceneManager.LoadScene(bossSceneIndex);
    }
}