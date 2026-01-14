using UnityEngine;
using UnityEngine.UI;

public class DestinationButton : MonoBehaviour
{
    public RectTransform destinationPoint;
    public MapController mapController;

    [Header("Level Einstellungen")]
    public int sceneIndexToLoad = 1;
    public int requiredProgress = 1;

    [Header("Design")]
    public Color lockedColor = Color.black; // Schwarz wenn gesperrt
    public Color unlockedColor = Color.red; // Rot wenn offen

    private Image buttonImage;
    private Button myButton;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        myButton = GetComponent<Button>();

        // Fortschritt laden
        int currentProgress = PlayerPrefs.GetInt("LevelProgress", 1);

        // --- Logik f¸r Farbe und Klickbarkeit ---
        if (currentProgress >= requiredProgress)
        {
            // FREIGESCHALTET
            buttonImage.color = unlockedColor;       // Rot
            myButton.interactable = true;            // Klickbar
            myButton.onClick.RemoveAllListeners();   // Sicherheitsmaﬂnahme: Alte Listener entfernen
            myButton.onClick.AddListener(OnButtonClick);
        }
        else
        {
            // GESPERRT
            buttonImage.color = lockedColor;         // Schwarz
            myButton.interactable = false;           // Nicht klickbar
            // Button bleibt aber sichtbar (SetActive bleibt true)
        }
    }

    void OnButtonClick()
    {
        mapController.MoveCharacterTo(destinationPoint, sceneIndexToLoad);
    }
}