using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class PointManager : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] public Slider silder;
    private float timeRemaining = 5;
    public int points;
    

    private void Update()
    {
        if(timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timeText.text = "Time: " + Mathf.CeilToInt(timeRemaining).ToString();
        }
        else
        {
            SceneManager.LoadScene("KoboldScene");
        }
    }

    public void AddPoints(int amount)
    {
        points += amount;
        silder.value = 0.1f;

        GameManager.Instance.sliderValue = slider.value;
        GameManager.Instance.points = points;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Acorn"))
        {
            AddPoints(1);
            Destroy(other.gameObject);
        }
    }

}
