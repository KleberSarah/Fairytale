using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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
