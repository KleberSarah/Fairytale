using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FightKobold : MonoBehaviour
{
    [SerializeField] private Slider lifeSlider;
    [SerializeField] private Slider mySlider;
    [SerializeField] private TMP_Text loseWinText;

    public LevelFinisher levelFinisher;

    private void Start()
    {
        // Score aus den PlayerPrefs laden
        int savedScore = PlayerPrefs.GetInt("Score", 0);
        
        // FEHLER BEHOBEN: Hier nehmen wir die puren Punkte (savedScore) 
        // und nicht mehr mal 0.1f! 
        mySlider.value = savedScore; 
    }

    public void FightPoints()
    {
        lifeSlider.value -= mySlider.value;
        mySlider.value = 0f;

        // Nach dem Klick Score sicherheitshalber nullen
        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.Save();

        if (lifeSlider.value <= 0)
        {
            Debug.Log("Kobold besiegt!");
            loseWinText.gameObject.SetActive(true);
            loseWinText.text = "Du hast gewonnen!";
            levelFinisher.CompleteLevel();
        }
        else
        {
            Debug.Log("Kobold hat noch Leben übrig: " + lifeSlider.value);
            loseWinText.gameObject.SetActive(true);
            loseWinText.text = "Du hast verloren!";
            levelFinisher.ReloadLevel();
        }
    }
}