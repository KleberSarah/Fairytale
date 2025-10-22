using UnityEngine;

public class Controller: MonoBehaviour
{
    [Header("Movement Settings")]
    public float ascendSpeed = 3f;        // Aufstiegsgeschwindigkeit
    public float horizontalSpeed = 2f;    // Seitliche Steuerungsgeschwindigkeit
    public float maxAscendSpeed = 5f;     // Maximalgeschwindigkeit nach oben
    public float gravity = -0.5f;         // Sanfte "Ballon-Schwerkraft" nach unten

    [Header("Rotation Settings")]
    public float turnSmoothTime = 0.1f;


    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
     
    }

    void Update()
    {
        // Input
        float horizontal = Input.GetAxis("Horizontal"); // A/D oder Pfeiltasten
     

        Vector3 moveDir = new Vector3(horizontal, 0f, 0f).normalized;


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
