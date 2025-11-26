using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PointManager : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private Slider silder;
    [SerializeField] private GameObject acorn;
    private float timeRemaining = 30;
    private int points;
    

    private void Update()
    {
        if(timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timeText.text = "Time: " + Mathf.CeilToInt(timeRemaining).ToString();
        }
        else
        {
            timeText.text = "0";
        }

        
    }

    public void AddPoints()
    {
        silder.value += 0.1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Acorn"))
        {
            AddPoints();
            Destroy(other.gameObject);
        }
    }

}
