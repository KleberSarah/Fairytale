using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PointManager : MonoBehaviour
{
    public static PointManager Instance;

    public TMP_Text timeText;
    public Slider slider;

    [Header("Einstellungen")]
    public int index; 
    public float timeRemaining = 45f;
    public int points;
    
    private bool isGameActive = true;

    private bool keepPointsForNextScene = false;


    public void Awake()
    {
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

    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Wird beim Szenenwechsel aufgerufen
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        if (keepPointsForNextScene)
        {
            
            keepPointsForNextScene = false;
            
            // Nur UI-Elemente neu verknüpfen
            FindUIElements();
        }
        else
        {
            // Normaler Level-Start oder Tod (Reload): Alles auf 0 setzen
            ResetGameValues();
            FindUIElements();
        }
    }

    // Diese Methode setzt Punkte und Zeit auf Anfangswert
    public void ResetGameValues()
    {
        points = 0;
        timeRemaining = 45f;
        isGameActive = true;

        // UI-Referenzen löschen, da sie zur alten Szene gehörten
        timeText = null;
        slider = null;
    }

    // --- SCHNITTSTELLE FÜR MANUELLE ZUWEISUNG ---
    public void SetUI(TMP_Text txt, Slider sld)
    {
        timeText = txt;
        slider = sld;

        // Slider sofort initialisieren, wenn vorhanden
        if (slider != null)
        {
            slider.minValue = 0;
            slider.maxValue = 1;
            // ANPASSUNG: Hier den aktuellen Punktestand setzen statt hart auf 0
            slider.value = points * 0.1f; 
        }

        // Text sofort initialisieren
        if (timeText != null)
        {
            timeText.text = "Time: " + Mathf.CeilToInt(timeRemaining).ToString();
        }
    }
    // --------------------------------------------

    public void Update()
    {
        if (!isGameActive) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

            // Nur updaten, wenn uns jemand einen Text zugewiesen hat
            if (timeText != null)
            {
                timeText.text = "Time: " + Mathf.CeilToInt(timeRemaining).ToString();
            }
        }
        else
        {
            StartKoboldFight();
        }
    }
    

    public void StartKoboldFight()
    {
        isGameActive = false;
        keepPointsForNextScene = true; 
        
        SceneManager.LoadScene(index);
    }

    public void AddPoints(int amount)
    {
        points += amount;

        if (slider != null)
        {
            slider.value = points * 0.1f;
        }
    }

    public void FindUIElements()
    {
        var sliderObj = GameObject.FindWithTag("PointSlider");
        if (sliderObj != null) slider = sliderObj.GetComponent<Slider>();
        
        var textObj = GameObject.FindWithTag("TimeText");
        if (textObj != null) timeText = textObj.GetComponent<TMP_Text>();
        
        // Initialisierung
        if (slider != null) 
        {
            slider.minValue = 0;
            slider.maxValue = 1;
            slider.value = points * 0.1f;
        }
    }
}