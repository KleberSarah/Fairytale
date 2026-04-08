using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryScroller : MonoBehaviour
{
    [Header("UI Referenzen")]
    [Tooltip("Ziehe hier das RectTransform deines Textes (oder des Brief-Hintergrunds) rein.")]
    public RectTransform letterRect;

    [Header("Scroll Einstellungen")]
    public float scrollSpeed = 100f;
    [Tooltip("Die Y-Position, bei der der Text als 'fertig gelesen' gilt und die Szene wechselt.")]
    public float endYPosition = 1500f; 

    [Header("Szenenwechsel")]
    public int mapSceneIndex; // Index deiner Weltkarten-Szene

    private bool hasFinished = false;

    void Update()
    {
        // Wenn wir schon wechseln, mach nichts mehr
        if (hasFinished) return;

        // 1. Den Brief stetig nach oben bewegen
        letterRect.anchoredPosition += new Vector2(0f, scrollSpeed * Time.deltaTime);

        // 2. Prüfen, ob der Brief komplett durchgescrollt ist
        if (letterRect.anchoredPosition.y >= endYPosition)
        {
            FinishStory();
        }

        // 3. OPTIONAL: Der Spieler kann mit Linksklick oder Leertaste überspringen
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            FinishStory();
        }
    }

    private void FinishStory()
    {
        hasFinished = true;
        
        // Zur Weltkarte wechseln
        SceneManager.LoadScene(mapSceneIndex);
    }
}