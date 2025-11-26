using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class PointManager : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] public Slider slider;
    public GameObject mySlider;
    private float timeRemaining = 10;
    public static int points;
    public PointManager pointManager;



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

        DontDestroyOnLoad(mySlider);
        DontDestroyOnLoad(pointManager);

    }

    public void AddPoints(int amount)
    {
        points += amount;
        slider.value += 0.1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(CompareTag("Player") & other.CompareTag("Acorn"))
        {
            AddPoints(1);
            Destroy(other.gameObject);
        }
    }

}
