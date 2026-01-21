using System.Collections;
using UnityEngine;

public class WindEmitter : MonoBehaviour
{
    [Header("Wind-Timing")]
    public float minWaitTime = 1.5f;
    public float maxWaitTime = 4f;
    public float windDuration = 1.2f;

    [Header("Wind-Stärke & Bereich")]
    public float windPower = 15f;
    public float maxDistance = 10f; // Sicherheitshalber: Wie weit reicht der Wind?
    public Vector3 detectionBoxSize = new Vector3(5f, 5f, 5f);
    public Vector3 boxOffset = new Vector3(0, 0, 2.5f);

    [Header("Referenzen")]
    public GameObject playerObject; // Ziehe hier dein Spieler-Objekt rein
    public ParticleSystem windParticles;

    private bool isWindActive = false;

    void Start()
    {
        if (playerObject == null)
            Debug.LogError("WINDSTATION: Kein Spieler-Objekt zugewiesen!");

        if (windParticles != null)
            windParticles.Stop();

        StartCoroutine(WindCycle());
    }

    IEnumerator WindCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));

            isWindActive = true;
            Debug.Log("WIND: Startet jetzt!"); // Schau in die Console!
            if (windParticles != null) windParticles.Play();

            yield return new WaitForSeconds(windDuration);

            isWindActive = false;
            Debug.Log("WIND: Ende.");
            if (windParticles != null) windParticles.Stop();
        }
    }

    void Update()
    {
        if (isWindActive && playerObject != null)
        {
            if (IsPlayerInZone())
            {
                // Verschiebe den Spieler
                playerObject.transform.position += transform.forward * windPower * Time.deltaTime;
                Debug.Log("WIND: Ich schiebe den Spieler gerade!");
            }
        }
    }

    bool IsPlayerInZone()
    {
        // 1. Einfacher Distanz-Check (Sicherheit)
        float dist = Vector3.Distance(transform.position, playerObject.transform.position);
        if (dist > maxDistance) return false;

        // 2. Box-Check (Relativ zur Rotation der Station)
        // Wir wandeln die Welt-Position des Spielers in die lokale Position der Station um
        Vector3 localPlayerPos = transform.InverseTransformPoint(playerObject.transform.position);

        // Wir prüfen, ob diese lokale Position innerhalb unserer Box (mit Offset) liegt
        Vector3 boxCenter = boxOffset;
        bool inX = Mathf.Abs(localPlayerPos.x - boxCenter.x) < detectionBoxSize.x / 2;
        bool inY = Mathf.Abs(localPlayerPos.y - boxCenter.y) < detectionBoxSize.y / 2;
        bool inZ = Mathf.Abs(localPlayerPos.z - boxCenter.z) < detectionBoxSize.z / 2;

        return inX && inY && inZ;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // Zeichne die Box im Editor
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = isWindActive ? Color.red : Color.cyan;
        Gizmos.DrawWireCube(boxOffset, detectionBoxSize);

        // Zeige die Windrichtung (Blauer Pfeil in Unity = transform.forward)
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(Vector3.zero, Vector3.forward * 3f);
    }
#endif
}
