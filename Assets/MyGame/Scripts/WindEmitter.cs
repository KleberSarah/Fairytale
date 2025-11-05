using UnityEngine;
using System.Collections;

public class WindEmitter : MonoBehaviour
{
    [Header("Wind Einstellungen")]
    public float minDelay = 2f;          // Mindestzeit zwischen Windstößen
    public float maxDelay = 6f;          // Maximalzeit zwischen Windstößen
    public float windForce = 10f;        // Stärke des Windstoßes
    public float windDuration = 0.5f;    // Wie lange der Wind anhält
    public float windRange = 10f;        // Reichweite, in der der Player getroffen wird

    [Header("Referenzen")]
    public Transform player;             // Player-Objekt, das den Controller hat

    private bool isBlowing = false;

    void Start()
    {
        StartCoroutine(WindRoutine());
    }

    IEnumerator WindRoutine()
    {
        while (true)
        {
            // Zufällige Zeit bis zum nächsten Windstoß
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);

            // Windstoß erzeugen
            StartCoroutine(BlowWind());
        }
    }

    IEnumerator BlowWind()
    {
        if (player == null) yield break;

        // Prüfen, ob der Player in Reichweite ist
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > windRange) yield break;

        isBlowing = true;

        // Richtung vom WindEmitter zum Player (um ihn wegzuschubsen)
        Vector3 dir = (player.position - transform.position).normalized;

        // Den Controller holen
        Controller playerController = player.GetComponent<Controller>();
        CharacterController cc = player.GetComponent<CharacterController>();

        if (playerController != null && cc != null)
        {
            float timer = 0f;
            while (timer < windDuration)
            {
                // Den Player direkt verschieben (Schub)
                cc.Move(dir * windForce * Time.deltaTime);
                timer += Time.deltaTime;
                yield return null;
            }
        }

        isBlowing = false;
    }

#if UNITY_EDITOR
    // Nur zur besseren Sichtbarkeit in der Szene
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
