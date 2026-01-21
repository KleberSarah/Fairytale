using UnityEngine;
using System.Collections;

public class WindEmitter : MonoBehaviour
{
    [Header("Wind-Timing")]
    public float minWaitTime = 1.5f;
    public float maxWaitTime = 4f;
    public float windDuration = 1.2f;

    [Header("Wind-Stärke & Bereich")]
    public float windPower = 20f;
    public Vector3 detectionBoxSize = new Vector3(5f, 5f, 8f);
    public Vector3 boxOffset = new Vector3(0, 0, 4f);

    [Header("Referenzen")]
    public ParticleSystem windParticles;

    private bool isWindActive = false;

    void Start()
    {
        if (windParticles != null) windParticles.Stop();
        StartCoroutine(WindCycle());
    }

    IEnumerator WindCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            isWindActive = true;
            if (windParticles != null) windParticles.Play();

            yield return new WaitForSeconds(windDuration);

            isWindActive = false;
            if (windParticles != null) windParticles.Stop();
        }
    }

    void Update()
    {
        if (isWindActive)
        {
            // Bereich im Weltraum berechnen (berücksichtigt Rotation der Station)
            Vector3 center = transform.TransformPoint(boxOffset);
            Collider[] hits = Physics.OverlapBox(center, detectionBoxSize / 2, transform.rotation);

            foreach (var hit in hits)
            {
                Controller player = hit.GetComponent<Controller>();
                if (player != null)
                {
                    // Wind weht immer in die "Vorwärts"-Richtung der Station (blaue Achse)
                    Vector3 windDir = transform.forward;
                    player.transform.position += windDir * windPower * Time.deltaTime;  
                    Debug.Log("Wind applied to player!");
                }
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // Zeichnet den Windbereich zur Kontrolle in den Editor
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = isWindActive ? Color.red : Color.cyan;
        Gizmos.DrawWireCube(boxOffset, detectionBoxSize);

        // Gelber Pfeil zeigt die Windrichtung
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(Vector3.zero, Vector3.forward * 2f);
        Gizmos.matrix = oldMatrix;
    }
#endif
}