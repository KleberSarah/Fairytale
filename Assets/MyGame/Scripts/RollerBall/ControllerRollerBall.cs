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

    

    private bool isGameOver = false; // Damit man nicht gewinnen UND sterben kann

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        winText.text = "";

      
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

   

    void OnTriggerEnter(Collider other)
    {
        if (isGameOver) return;

        

        if (other.gameObject.CompareTag("Death"))
        {
            ReloadCurrentLevel();
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
       
    }

    // Hilfsfunktion: Lädt das aktuelle Level neu (egal wie es heißt)
    void ReloadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}