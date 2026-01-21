using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class ControllerRollerBall : MonoBehaviour
{
    public float speed;
    public float jumpHeight;

    private Rigidbody rb;
    public int count;
    public TMP_Text winText;
    public TMP_Text countText;

    public TMP_Text timerText;
    public float startTime = 10f;
    private float currentTime;

    public int numPickups;

    // NEU: Referenz zum LevelFinisher-Skript (das auf dem "Ziel"-Objekt liegt)
    public LevelFinisher levelFinisher;

    private bool isGameOver = false; // Damit man nicht gewinnen UND sterben kann

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetCountText();
        winText.text = "";

        currentTime = startTime;
        StartCoroutine(Countdown());
    }

    void FixedUpdate()
    {
        // Bewegung nur erlauben, wenn das Spiel läuft
        if (isGameOver) return;

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);

        if (Input.GetKeyDown("space"))
        {
            Vector3 jump = new Vector3(0.0f, jumpHeight, 0.0f);
            rb.AddForce(jump);
        }
    }

    IEnumerator Countdown()
    {
        while (currentTime > 0 && !isGameOver)
        {
            currentTime -= Time.deltaTime;
            // Verhindert negative Zahlen in der Anzeige
            timerText.text = "Time: " + Mathf.Ceil(Mathf.Max(0, currentTime)).ToString();
            yield return null;
        }

        // Wenn Zeit abgelaufen und Spiel noch nicht gewonnen
        if (!isGameOver && currentTime <= 0)
        {
            ReloadCurrentLevel();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isGameOver) return;

        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
            // Debug.Log(count + " tags gesammelt"); // Optional auskommentiert
            SetCountText();
        }

        if (other.gameObject.CompareTag("Death"))
        {
            ReloadCurrentLevel();
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();

        // Gewonnen-Check
        if (count >= numPickups && !isGameOver)
        {
            StartCoroutine(WinSequence());
        }
    }

    // NEU: Eine Coroutine für den Gewinn-Ablauf
    IEnumerator WinSequence()
    {
        isGameOver = true; // Stoppt Timer und Bewegung
        winText.text = "You win!";

        // Warte 2 Sekunden, damit der Spieler den Text lesen kann
        yield return new WaitForSeconds(2f);

        // Rufe das Finisher-Skript auf, um zu speichern und zur Map zu gehen
        if (levelFinisher != null)
        {
            levelFinisher.CompleteLevel();
        }
        else
        {
            Debug.LogError("LevelFinisher ist im Inspector nicht zugewiesen!");
        }
    }

    // Hilfsfunktion: Lädt das aktuelle Level neu (egal wie es heißt)
    void ReloadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}