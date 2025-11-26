using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float sliderValue;   // Was du speichern willst
    public int points;           // optional

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // bleibt beim Scene-Wechsel erhalten
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
