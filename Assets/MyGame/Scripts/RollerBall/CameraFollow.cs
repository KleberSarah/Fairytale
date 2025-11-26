using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;   // Der Ball
    public Vector3 offset;     // Abstand zur Kamera

    void LateUpdate()
    {
        // Kamera folgt der Position des Balls
        transform.position = target.position + offset;

        // Optional: Kamera soll immer auf den Ball schauen
        // transform.LookAt(target);
    }
}

