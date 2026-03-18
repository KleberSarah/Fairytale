using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public class SoundSlot
    {
        public string name;      // Der Name, mit dem du den Sound aufrufst
        public AudioClip clip;   // Die Audio-Datei
        [Range(0f, 1f)] public float volume = 1f;
        public bool isMusic = false; // Wenn wahr, wird es geloopt
    }

    [Header("Deine Sound Liste")]
    public List<SoundSlot> sounds;

    private AudioSource musicSource;
    private List<AudioSource> sfxSources = new List<AudioSource>();

    void Start()
    {
        // Ersetze "BackgroundMusic" durch den exakten Namen, 
        // den du im Inspector in der Liste eingetragen hast!
        Play("BackgroundMusic");
    }

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); return; }

        // Musik-Quelle vorbereiten
        musicSource = gameObject.AddComponent<AudioSource>();

        // Ein paar SFX-Quellen vorbereiten (Pool)
        for (int i = 0; i < 5; i++)
        {
            sfxSources.Add(gameObject.AddComponent<AudioSource>());
        }
    }

    public void Play(string soundName)
    {
        SoundSlot s = sounds.Find(x => x.name == soundName);

        if (s == null)
        {
            Debug.LogWarning("Sound " + soundName + " wurde nicht in der Liste gefunden!");
            return;
        }

        if (s.isMusic)
        {
            musicSource.clip = s.clip;
            musicSource.volume = s.volume;
            musicSource.loop = true;
            musicSource.Play();
        }
        else
        {
            // Suche eine freie Quelle für den Effekt
            AudioSource freeSource = sfxSources.Find(x => !x.isPlaying);
            if (freeSource == null) freeSource = sfxSources[0]; // Nutze erste, falls alle voll

            freeSource.clip = s.clip;
            freeSource.volume = s.volume;
            freeSource.Play();
        }
    }
}