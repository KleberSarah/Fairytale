using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public class SoundSlot
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        public bool isMusic = false;
        public bool loop = false; // NEU: Damit auch SFX loopen können
    }

    [Header("Deine Sound Liste")]
    public List<SoundSlot> sounds;

    private AudioSource musicSource;
    private List<AudioSource> sfxSources = new List<AudioSource>();

    void Awake()
    {
        if (Instance == null) { Instance = this;  }
        else { Destroy(gameObject); return; }

        // Musik-Quelle (für den Haupt-Track)
        musicSource = gameObject.AddComponent<AudioSource>();

        // SFX-Pool (für alles, was gleichzeitig klingen soll)
        for (int i = 0; i < 10; i++) // Auf 10 erhöht für mehr Gleichzeitigkeit
        {
            sfxSources.Add(gameObject.AddComponent<AudioSource>());
        }
    }

    void Start()
    {
        // Beispiel-Aufruf
        Play("Musik");
        Play("Wind");
    }

    public void Play(string soundName)
    {
        SoundSlot s = sounds.Find(x => x.name == soundName);

        if (s == null)
        {
            Debug.LogWarning("Sound '" + soundName + "' nicht gefunden!");
            return;
        }

        if (s.isMusic)
        {
            // Musik überschreibt sich immer selbst (nur 1 Track gleichzeitig)
            musicSource.clip = s.clip;
            musicSource.volume = s.volume;
            musicSource.loop = true;
            musicSource.Play();
        }
        else
        {
            // SFX suchen eine freie Quelle im Pool
            AudioSource freeSource = sfxSources.Find(x => !x.isPlaying);

            // Falls alle voll sind, nimm die erste (Notlösung)
            if (freeSource == null) freeSource = sfxSources[0];

            freeSource.clip = s.clip;
            freeSource.volume = s.volume;
            freeSource.loop = s.loop; // Nutzt die neue Loop-Einstellung
            freeSource.Play();
        }
    }
}