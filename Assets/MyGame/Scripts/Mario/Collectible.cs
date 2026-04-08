using UnityEngine;

public class Collectible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.Play("Collect");
            // Sucht den lokalen Manager in der Szene und gibt ihm den Punkt
            PointManager manager = Object.FindFirstObjectByType<PointManager>();
            if (manager != null)
            {
                manager.AddPoints(1);
            }
            
            Destroy(this.gameObject);
        }
    }
}