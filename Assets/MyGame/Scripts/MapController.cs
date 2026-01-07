using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MapController : MonoBehaviour
{
    public RectTransform character;
    public float moveSpeed = 300f;
    public Button levelStartButton;

    private bool isMoving = false;

    private void Start()
    {
        levelStartButton.gameObject.SetActive(false);
    }

    public void MoveCharacterTo(RectTransform destination)
    {
        if (!isMoving)
        {
            levelStartButton.gameObject.SetActive(false);
            StartCoroutine(MoveRoutine(destination));
        }
    }

    private IEnumerator MoveRoutine(RectTransform target)
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

        isMoving = false;
        levelStartButton.gameObject.SetActive(true);
    }
}
