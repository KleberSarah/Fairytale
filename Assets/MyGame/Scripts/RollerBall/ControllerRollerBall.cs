using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class ControllerRollerBall : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Wie stark der Ball beschleunigt")]
    public float speed = 20f; 
    [Tooltip("Maximale Rollgeschwindigkeit")]
    public float maxSpeed = 8f; 
    [Tooltip("Wie schnell der Ball bremst, wenn man keine Taste drückt (0 = gar nicht, 1 = sofort)")]
    [Range(0f, 1f)]
    public float brakingFactor = 0.9f; 

    [Header("Jump Settings")]
    public float jumpForce = 5f;
    [Tooltip("Wie weit der unsichtbare Strahl nach unten sucht, um den Boden zu erkennen.")]
    public float groundCheckDistance = 0.6f; 

    private Rigidbody rb;
    private SphereCollider sphereCollider;
    private float moveHorizontal;
    private float moveVertical;
    private bool jumpRequested = false;

    [Header("Game Logic")]
    public int numPickups; 
    private int count;     
    private bool isGameOver = false;

    [Header("UI References (Lokal)")]
    public TMP_Text winText;
    public TMP_Text countText;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();

        // Damit der Ball nicht ewig weiterrollt, wenn er anstößt
        rb.maxAngularVelocity = 20f; 

        if (winText != null) winText.text = "";
        UpdateCountText();
    }

    // Input IMMER in Update() abfragen, sonst werden Tastendrücke verschluckt!
    void Update()
    {
        if (isGameOver) return;

        // GetAxisRaw gibt direkt -1, 0 oder 1 zurück (kein schwammiges Beschleunigen)
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        // Sprung anfragen
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            jumpRequested = true;
        }
    }

    // Physik IMMER in FixedUpdate() anwenden
    void FixedUpdate()
    {
        if (isGameOver) return;

        // 1. Bewegung anwenden
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;
        rb.AddForce(movement * speed);

        // 2. Maximalgeschwindigkeit drosseln (nur X und Z Achse, damit Fallen normal funktioniert)
        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (flatVelocity.magnitude > maxSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(limitedVelocity.x, rb.linearVelocity.y, limitedVelocity.z);
        }

        // 3. Bremsen, wenn keine Taste gedrückt wird
        if (movement.magnitude == 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x * brakingFactor, rb.linearVelocity.y, rb.linearVelocity.z * brakingFactor);
        }

        // 4. Springen
        if (jumpRequested)
        {
            // Setzt vorherige vertikale Kräfte zurück, für knackigere Sprünge
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); 
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpRequested = false;
        }
    }

    // Prüft mittels Raycast (unsichtbarer Laserstrahl nach unten), ob wir den Boden berühren
    private bool IsGrounded()
    {
        // Startet in der Mitte des Balls und schießt einen Strahl nach unten. 
        // Die Distanz sollte minimal größer sein als der Radius des Balls (z.B. Radius 0.5 + 0.1 = 0.6)
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
    }

    void OnTriggerEnter(Collider other)
    {
        if (isGameOver) return;

        // --- 1. PICKUP LOGIK ---
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
            UpdateCountText();

            if (PointManager.Instance != null)
            {
                PointManager.Instance.AddPoints(1);
            }
            else
            {
                Debug.LogError("ACHTUNG: Kein PointManager in der Szene gefunden!");
            }

            if (count >= numPickups)
            {
                StartCoroutine(WinSequence());
            }
        }

        // --- 2. TOD LOGIK ---
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
        
        // Ball sofort stoppen bei Sieg
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (winText != null) winText.text = "You win!";
        yield return new WaitForSeconds(2f);
    }

    void ReloadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}