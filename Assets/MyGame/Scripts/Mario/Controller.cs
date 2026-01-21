using UnityEngine;

public class Controller : MonoBehaviour
{
    [Header("Movement Settings")]
    public float ascendSpeed = 3f;
    public float horizontalSpeed = 2f;
    public float gravity = -0.5f;

    private CharacterController controller;
    private Vector3 velocity;

    // Von außen steuerbar
    [HideInInspector] public Vector3 windEffect;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 1. Input holen
        float horizontal = Input.GetAxis("Horizontal");

        // 2. Aufstieg berechnen
        velocity.y += gravity * Time.deltaTime;
        if (velocity.y < ascendSpeed)
        {
            velocity.y = Mathf.Lerp(velocity.y, ascendSpeed, Time.deltaTime);
        }

        // 3. ALLE Bewegungen addieren
        // horizontaler Input + Aufstieg + Windkraft
        Vector3 move = new Vector3(horizontal * horizontalSpeed, velocity.y, 0f) + windEffect;

        // 4. Den CharacterController bewegen
        controller.Move(move * Time.deltaTime);

        // 5. Windkraft sofort wieder dämpfen (Brems-Effekt)
        windEffect = Vector3.Lerp(windEffect, Vector3.zero, Time.deltaTime * 5f);
    }
}