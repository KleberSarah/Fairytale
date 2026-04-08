using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class ControllerRollerBall : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float airSpeed = 7f;
    [Tooltip("Wie schnell der Ball ausrollt (höher = stoppt schneller)")]
    public float deceleration = 5f;

    [Header("Jump Settings")]
    public float jumpForce = 6f;
    public float groundCheckDistance = 0.6f;

    private Rigidbody rb;
    private Camera mainCamera;
    private bool isGameOver = false;

    [Header("UI References")]
    public TMP_Text winText;
    public TMP_Text countText;
    private int count;
    public int numPickups;
    public PointManager pointManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        mainCamera = Camera.main;

        if (winText != null) winText.text = "";
        UpdateCountText();
    }

    void Update()
    {
        if (isGameOver) return;

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        if (isGameOver) return;

        MoveBall();
    }

    void MoveBall()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 camForward = mainCamera.transform.forward;
        Vector3 camRight = mainCamera.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = (camForward * v + camRight * h).normalized;
        float targetSpeed = IsGrounded() ? moveSpeed : airSpeed;

        Vector3 currentHorizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        if (moveDir.magnitude > 0.1f)
        {
            rb.linearVelocity = new Vector3(moveDir.x * targetSpeed, rb.linearVelocity.y, moveDir.z * targetSpeed);
            rb.angularVelocity = new Vector3(moveDir.z * targetSpeed, 0, -moveDir.x * targetSpeed);
        }
        else
        {
            Vector3 lerpedVelocity = Vector3.Lerp(currentHorizontalVelocity, Vector3.zero, Time.fixedDeltaTime * deceleration);
            rb.linearVelocity = new Vector3(lerpedVelocity.x, rb.linearVelocity.y, lerpedVelocity.z);
            rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, Time.fixedDeltaTime * deceleration);
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
            UpdateCountText();
            if (count >= numPickups) StartCoroutine(WinSequence());
        }
        
        if (other.CompareTag("Death")) 
        {
            // TAFEL-LOGIK: Death -> Score = 0
            PlayerPrefs.SetInt("Score", 0);
            PlayerPrefs.Save();
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

         if (other.CompareTag("Finish")) 
        {
           pointManager.GoToBossFight();
        }


    }

    void UpdateCountText() => countText.text = "Count: " + count;

    IEnumerator WinSequence()
    {
        isGameOver = true;
        rb.linearVelocity = Vector3.zero;
        winText.text = "You win!";
        yield return new WaitForSeconds(2f);
    }
}