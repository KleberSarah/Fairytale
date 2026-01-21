using UnityEngine;
using System.Collections;

public class WindEmitter : MonoBehaviour
{
    [Header("Referenzen")]
    public ParticleSystem windParticles;

    [Header("Wind Einstellungen")]
    public Vector3 windDirection = Vector3.right;
    public float windStrength = 20f;

    [Header("Timing Einstellungen")]
    public float particleLeadTime = 0.5f; // Zeit, die Partikel brauchen um sichtbar zu sein
    public float minActiveTime = 1.5f;    // Wie lange die Kraft wirkt
    public float maxActiveTime = 3f;
    public float minWaitTime = 2f;        // Pause zwischen Schüssen
    public float maxWaitTime = 5f;

    private bool isActive = false;

    void Start()
    {
        if (windParticles != null) windParticles.Stop();
        StartCoroutine(WindCycle());
    }

    IEnumerator WindCycle()
    {
        while (true)
        {
            // 1. PAUSE (Alles aus)
            isActive = false;
            if (windParticles != null) windParticles.Stop();
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));

            // 2. VORLAUF (Partikel starten, aber noch keine Kraft)
            if (windParticles != null) windParticles.Play();
            Debug.Log("Partikel starten...");
            yield return new WaitForSeconds(particleLeadTime);

            // 3. FEUERN (Jetzt ist die Kraft aktiv)
            isActive = true;
            Debug.Log("Windkraft aktiv!");
            yield return new WaitForSeconds(Random.Range(minActiveTime, maxActiveTime));

            // 4. AUSKLINGEN (Optional: Kraft stoppen, bevor Partikel verschwinden)
            isActive = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isActive) ApplyWindForce(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (isActive) ApplyWindForce(other);
    }

    private void ApplyWindForce(Collider other)
    {
        Controller player = other.GetComponent<Controller>();
        if (player != null)
        {
            player.windEffect = windDirection.normalized * windStrength;
        }
    }
}