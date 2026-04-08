using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections; // Wichtig für die Coroutine

public class FightKobold : MonoBehaviour
{
    [SerializeField] private Slider lifeSlider;
    [SerializeField] private Slider mySlider;
    [SerializeField] private TMP_Text loseWinText;

    public LevelFinisher levelFinisher;

    [Header("Kampf Animation")]
    public GameObject attackObject;      // Das Objekt, das fliegt (z.B. ein Lichtball)
    public Transform fairyTransform;    // Startpunkt
    public Transform koboldTransform;   // Zielpunkt
    public float attackSpeed = 10f;     // Fluggeschwindigkeit

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
        // Wir starten die Coroutine statt den Code direkt auszuführen
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        // 1. Objekt erzeugen an der Position der Fee
        GameObject projectile = Instantiate(attackObject, fairyTransform.position, Quaternion.identity);

        // 2. Objekt zum Kobold bewegen
        while (Vector3.Distance(projectile.transform.position, koboldTransform.position) > 0.2f)
        {
            projectile.transform.position = Vector3.MoveTowards(
                projectile.transform.position,
                koboldTransform.position,
                attackSpeed * Time.deltaTime
            );
            yield return null; // Warte einen Frame
        }

        // 3. Kurz warten wenn angekommen
        Destroy(projectile);
        yield return new WaitForSeconds(0.2f);

        // --- AB HIER DER ORIGINALE CODE ---
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