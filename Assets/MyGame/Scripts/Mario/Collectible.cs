using UnityEngine;

public class Collectible : MonoBehaviour
{
    public PointManager pointManager;
   

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == ("Player"))
        {
            pointManager.AddPoints(1);
            Destroy(this.gameObject);
        }
    }
}
