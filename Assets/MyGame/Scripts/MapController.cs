using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic; // Nötig für Listen
using UnityEngine.SceneManagement;

public class MapController : MonoBehaviour
{
    public RectTransform character;
    public float moveSpeed = 300f;

    // NEU: Eine Liste aller Punkte, um den Startpunkt zu finden
    // Ziehe hier alle deine DestinationButtons (die RectTransforms) rein!
    // Reihenfolge: Index 0 = Start, Index 1 = Level 1 Ziel, Index 2 = Level 2 Ziel
    public List<RectTransform> levelPoints;

    private bool isMoving = false;

    private void Start()
    {
        UpdateCharacterPosition();
    }

    // Setzt den Charakter sofort an die richtige Stelle basierend auf dem Fortschritt
    private void UpdateCharacterPosition()
    {
        int currentProgress = PlayerPrefs.GetInt("LevelProgress", 1);

        // Wir berechnen den Index für die Liste.
        // Wenn Progress = 1 (Neues Spiel), wollen wir Index 0 (Startpunkt).
        // Wenn Progress = 2 (Level 1 fertig), wollen wir Index 1 (Level 1 Punkt).
        int listIndex = currentProgress - 1;

        // Sicherheitscheck, damit wir nicht auf einen Index zugreifen, den es nicht gibt
        if (listIndex >= 0 && listIndex < levelPoints.Count)
        {
            character.anchoredPosition = levelPoints[listIndex].anchoredPosition;
        }
    }

    public void MoveCharacterTo(RectTransform destination, int sceneIndex)
    {
        if (!isMoving)
        {
            StartCoroutine(MoveRoutine(destination, sceneIndex));
        }
    }

    private IEnumerator MoveRoutine(RectTransform target, int sceneIndex)
    {
        isMoving = true;

        while (Vector2.Distance(character.anchoredPosition, target.anchoredPosition) > 1f)
        {
            character.anchoredPosition = Vector2.MoveTowards(
                character.anchoredPosition,
                target.anchoredPosition,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        character.anchoredPosition = target.anchoredPosition;
        isMoving = false;

        yield return new WaitForSeconds(0.5f);

        Debug.Log("Lade Szene mit Index: " + sceneIndex);
        SceneManager.LoadScene(sceneIndex);
    }
}