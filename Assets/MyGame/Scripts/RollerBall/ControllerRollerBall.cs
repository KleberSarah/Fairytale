using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class ControllerRollerBall : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;
    public float jumpHeight = 5f;
    private Rigidbody rb;

    [Header("Game Logic")]
    public int numPickups; // Wie viele Pickups gibt es insgesamt?
    private int count;     // Eigener Zähler für Win-Condition
    private bool isGameOver = false;

    [Header("UI References (Lokal)")]
    public TMP_Text winText;
    public TMP_Text countText;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (winText != null) winText.text = "";
        UpdateCountText();
    }

    void FixedUpdate()
    {
        if (isGameOver) return;

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);

        // Verbesserte Sprunglogik
        if (Input.GetButtonDown("Jump")) // Standard ist Leertaste
        {
            // ForceMode.Impulse ist besser für knackiges Springen
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isGameOver) return;

        // --- 1. PICKUP LOGIK ---
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);

            // Lokalen Zähler erhöhen
            count++;
            UpdateCountText();

            // GLOBALEN Manager informieren (für den Slider!)
            if (PointManager.Instance != null)
            {
                PointManager.Instance.AddPoints(1);
            }
            else
            {
                Debug.LogError("ACHTUNG: Kein PointManager in der Szene gefunden!");
            }

            // Win Condition prüfen
            if (count >= numPickups)
            {
                StartCoroutine(WinSequence());
            }
        }

        // --- 2. TOD LOGIK ---
        // Stelle sicher, dass deine Fallen den Tag "Death" haben!
        if (other.gameObject.CompareTag("Death"))
        {
            ReloadCurrentLevel();
        }
    }

    void UpdateCountText()
    {
        if (countText != null)
        {
            countText.text = "Count: " + count.ToString();
        }
    }

    IEnumerator WinSequence()
    {
        isGameOver = true;
        if (winText != null) winText.text = "You win!";

        // Kurze Pause zum Lesen
        yield return new WaitForSeconds(2f);

        // Hier ggf. nächstes Level laden
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void ReloadCurrentLevel()
    {
        // Lädt die Szene neu. Der PointManager bemerkt das und resettet sich selbst.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}