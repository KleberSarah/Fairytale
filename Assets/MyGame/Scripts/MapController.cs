using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MapController : MonoBehaviour
{
    public Transform character; // Charakter Transform
    public Animator characterAnimator;
    public float moveSpeed = 3f;
    public Button levelStartButton; // Level-Start Button

    private bool isMoving = false;

    private void Start()
    {
        // Level-Start Button zu Beginn deaktivieren
        levelStartButton.gameObject.SetActive(false);
    }

    public void MoveCharacterTo(Transform destination)
    {
        if (!isMoving)
        {
            // Button deaktivieren während Bewegung
            levelStartButton.gameObject.SetActive(false);
            StartCoroutine(MoveRoutine(destination.position));
        }
    }

    private IEnumerator MoveRoutine(Vector3 targetPos)
    {
        isMoving = true;
        characterAnimator.SetBool("isWalking", true);

        while (Vector3.Distance(character.position, targetPos) > 0.1f)
        {
            character.position = Vector3.MoveTowards(character.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        characterAnimator.SetBool("isWalking", false);
        isMoving = false;

        // Level-Start Button aktivieren, sobald Ziel erreicht
        levelStartButton.gameObject.SetActive(true);
    }
}
