using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PointManager : MonoBehaviour
{
    public static PointManager Instance; // Singleton, damit er überlebt

    [SerializeField] private TMP_Text timeText;
    [SerializeField] private Slider slider;

    private float timeRemaining = 30;
    public int points;

    private bool isGameActive = true; // Steuert, ob die Logik laufen soll

    private void Awake()
    {
        // Sicherstellen, dass nur ein PointManager existiert
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null); // Wichtig für den Fehler von vorhin!
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Wenn wir nicht mehr im "Sammel-Modus" sind, brich hier ab!
        if (!isGameActive) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

            // UI nur updaten, wenn die Referenzen noch da sind
            if (timeText != null)
                timeText.text = "Time: " + Mathf.CeilToInt(timeRemaining).ToString();
        }
        else
        {
            StartKoboldFight();
        }
    }

    private void StartKoboldFight()
    {
        isGameActive = false; // Stoppt die Update-Logik
        SceneManager.LoadScene("KoboldScene");
    }

    public void AddPoints(int amount)
    {
        points += amount;

        // Sicherheitsscheck: Nur UI bedienen, wenn sie existiert
        if (slider != null)
        {
            slider.value += 0.1f;
        }
    }
}