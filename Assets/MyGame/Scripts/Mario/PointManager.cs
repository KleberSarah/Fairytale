using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PointManager : MonoBehaviour
{
    public static PointManager Instance;

    // Diese Variablen werden NICHT mehr automatisch gef�llt.
    // Sie warten auf Zuweisung von au�en (durch SceneUISetup).
    public TMP_Text timeText;
    public Slider slider;

    [Header("Einstellungen")]
    public int index; // Szene, die geladen wird, wenn Zeit abl�uft
    public float timeRemaining = 45f;
    public int points;
    private bool isGameActive = true;


    private void Awake()
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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Wird beim Szenenwechsel aufgerufen
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // WICHTIG: Hier wird NICHTS mehr gesucht!
        // Wir setzen nur die Spielwerte zur�ck.
        ResetGameValues();
        FindUIElements();

    }

    // Diese Methode setzt Punkte und Zeit auf Anfangswert
    private void ResetGameValues()
    {
        points = 0;
        timeRemaining = 45f;
        isGameActive = true;

        // UI-Referenzen l�schen, da sie zur alten Szene geh�rten
        timeText = null;
        slider = null;
    }

    // --- SCHNITTSTELLE F�R MANUELLE ZUWEISUNG ---
    // Diese Methode muss vom "SceneUISetup"-Skript in der Szene aufgerufen werden
    public void SetUI(TMP_Text txt, Slider sld)
    {
        timeText = txt;
        slider = sld;

        // Slider sofort initialisieren, wenn vorhanden
        if (slider != null)
        {
            slider.minValue = 0;
            slider.maxValue = 1;
            slider.value = 0; // Start bei 0
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

    private void StartKoboldFight()
    {
        isGameActive = false;
        SceneManager.LoadScene(index);
        
    }

    public void AddPoints(int amount)
    {
        points += amount;

        // Nur updaten, wenn uns jemand einen Slider zugewiesen hat
        if (slider != null)
        {
            slider.value = points * 0.1f;
        }
    }
    private void FindUIElements()
    {
   
    
    var sliderObj = GameObject.FindWithTag("PointSlider");
    if (sliderObj != null) slider = sliderObj.GetComponent<Slider>();
    
    var textObj = GameObject.FindWithTag("TimeText");
    if (textObj != null) timeText = textObj.GetComponent<TMP_Text>();
    
    // Initialisierung wie in deinem SetUI
    if (slider != null) {
        slider.minValue = 0;
        slider.maxValue = 1;
        slider.value = 0;
    }

    }
}