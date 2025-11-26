using UnityEngine;
using System.Collections;

public class WindEmitter : MonoBehaviour
{
    [Header("Wind Einstellungen")]
    public float minDelay = 2f;
    public float maxDelay = 6f;
    public float windForce = 10f;
    public float windDuration = 0.5f;
    public float windRange = 10f;

    [Header("Referenzen")]
    public Transform player;
    public ParticleSystem windParticles;   // <–– dein Wind Particle System

    private bool isBlowing = false;
    private ParticleSystem.EmissionModule emission;

    void Start()
    {
        if (windParticles != null)
            emission = windParticles.emission;

        // Partikel am Anfang aus
        if (windParticles != null)
            emission.enabled = false;

        StartCoroutine(WindRoutine());
    }

    IEnumerator WindRoutine()
    {
        while (true)
        {
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);

            StartCoroutine(BlowWind());
        }
    }

    IEnumerator BlowWind()
    {
        if (player == null) yield break;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > windRange) yield break;

        isBlowing = true;

        // Partikel einschalten
        if (windParticles != null)
        {
            emission.enabled = true;
            windParticles.Play();
        }

        Vector3 dir = (player.position - transform.position).normalized;

        CharacterController cc = player.GetComponent<CharacterController>();

        float timer = 0f;
        while (timer < windDuration)
        {
            if (cc != null)
                cc.Move(dir * windForce * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }

        isBlowing = false;

        // Partikel wieder ausschalten
        if (windParticles != null)
        {
            emission.enabled = false;
            windParticles.Stop();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, windRange);

        if (isBlowing)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(transform.position, 0.5f);
        }
    }
#endif
}
