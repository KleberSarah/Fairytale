using UnityEngine;

using UnityEngine.UI;

using TMPro;
using UnityEngine.SceneManagement;



public class FightKobold : MonoBehaviour

{

    [SerializeField] private Slider lifeSlider;

    [SerializeField] private Slider mySlider;

    [SerializeField] private TMP_Text loseWinText;

    public LevelFinisher levelFinisher;
    public PointManager pointManager;
    







    private void Start()

    {

        mySlider.value = PointManager.Instance.points;

    }

    
    

    public void FightPoints()

    {



        lifeSlider.value -= mySlider.value;

        mySlider.value = 0f;



        if (lifeSlider.value <= 0)

        {

            Debug.Log("Kobold besiegt!");

            loseWinText.gameObject.SetActive(true);

            loseWinText.text = "Du hast gewonnen!";

            levelFinisher.CompleteLevel();

        }

        else

        {

            Debug.Log("Kobold hat noch Leben �brig: " + lifeSlider.value);

            loseWinText.gameObject.SetActive(true);

            loseWinText.text = "Du hast verloren!";

        
            levelFinisher.ReloadLevel();
    
            

        }

    }

}