using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PointManager : MonoBehaviour
{
    public static PointManager Instance;

    private TMP_Text timeText;
    private Slider slider;

    public int index; // Index der Szene, die nach Ablauf der Zeit kommt
    private float timeRemaining = 30f;
    public int points;
    private bool isGameActive = true;

    private void Awake()
    {
        // Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Wird IMMER ausgeführt, wenn eine Szene startet (auch beim Reload/Tod)
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 1. UI neu finden
        timeText = Object.FindAnyObjectByType<TMP_Text>();
        slider = Object.FindAnyObjectByType<Slider>();

        // 2. Slider technisch korrekt einstellen (WICHTIG!)
        if (slider != null)
        {
            slider.minValue = 0;
            slider.maxValue = 1; // Wir normalisieren den Slider auf 0 bis 1
        }

        // 3. Spielwerte zurücksetzen
        ResetManager();
    }

    private void ResetManager()
    {
        points = 0;
        timeRemaining = 30f;
        isGameActive = true;

        if (slider != null)
        {
            slider.value = 0;
        }

        Debug.Log("Manager resettet. Punkte: 0");
    }

    private void Update()
    {
        if (!isGameActive) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
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
        isGameActive = false;
        SceneManager.LoadScene(index);
    }

    public void AddPoints(int amount)
    {
        points += amount;

        // Debugging: Sehen wir im Log, ob Punkte ankommen?
        Debug.Log("Punkt hinzugefügt. Gesamt: " + points);

        if (slider != null)
        {
            // Da MaxValue jetzt sicher 1 ist, füllt 0.1 den Balken um 10%
            slider.value = points * 0.1f;
        }
        else
        {
            Debug.LogWarning("Kein Slider gefunden!");
        }
    }
}