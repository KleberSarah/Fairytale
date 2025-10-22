using UnityEngine;

public class BalloonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float ascendSpeed = 3f;        // Aufstiegsgeschwindigkeit
    public float horizontalSpeed = 2f;    // Seitliche Steuerungsgeschwindigkeit
    public float maxAscendSpeed = 5f;     // Maximalgeschwindigkeit nach oben
    public float gravity = -0.5f;         // Sanfte "Ballon-Schwerkraft" nach unten

    [Header("Rotation Settings")]
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("Bitte füge einen CharacterController zum Player hinzu!");
        }
    }

    void Update()
    {
        // Input
        float horizontal = Input.GetAxis("Horizontal"); // A/D oder Pfeiltasten
        float vertical = Input.GetAxis("Vertical");     // W/S oder Pfeiltasten

        // Richtung relativ zur Kamera
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 moveDir = Vector3.zero;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }

        // Bewegung nach oben (Ballonaufstieg)
        velocity.y += gravity * Time.deltaTime; // leichte Gravitation
        if (velocity.y < ascendSpeed)
        {
            velocity.y = Mathf.Lerp(velocity.y, ascendSpeed, Time.deltaTime);
        }

        // Bewegung zusammenführen
        Vector3 move = moveDir * horizontalSpeed + new Vector3(0f, velocity.y, 0f);
        controller.Move(move * Time.deltaTime);
    }
}
