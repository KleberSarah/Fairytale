using UnityEngine;

public class Collectible : MonoBehaviour
{
    public PointManager pointManager;
    public string collectSoundName = "Pickup"; // Der Name des Sounds im AudioManager

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. Punkte hinzufügen
            if (pointManager != null)
            {
                pointManager.AddPoints(1);
            }

            // 2. Sound abspielen über die statische Instanz des AudioManagers
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.Play(collectSoundName);
            }
            else
            {
                Debug.LogWarning("AudioManager wurde in der Szene nicht gefunden!");
            }

            // 3. Objekt zerstören
            Destroy(this.gameObject);
        }
    }
}