using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds, ambientSounds;
    public AudioSource musicSource, sfxSource, loopSfxSource, ambientSource;

    [SerializeField]
    AudioLowPassFilter MusicAudioLowPassFilter;
    [SerializeField]
    AudioLowPassFilter AmbientAudioLowPassFilter;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Mo¿esz tu dodaæ kod inicjalizacyjny, jeœli jest potrzebny.
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void StopMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            musicSource.Stop();
        }
    }
    public void PlayAmbient(string name)
    {
        Sound s = Array.Find(ambientSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            ambientSource.clip = s.clip;
            ambientSource.Play();
        }
    }

    public void StopAmbient(string name)
    {
        Sound s = Array.Find(ambientSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            ambientSource.Stop();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("SFX Not Found");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void PlaySFX(AudioClip name)
    {
       
            sfxSource.PlayOneShot(name);
        
    }
    public void PlaySFXAtSource(AudioClip name, AudioSource source)
    {
        source.volume = sfxSource.volume;
        source.PlayOneShot(name);
        

    }

    public void PlaySFXLoop(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("SFX Not Found");
        }
        else
        {
            loopSfxSource.clip = s.clip;
            loopSfxSource.loop = true;
            loopSfxSource.Play();
        }
    }

    public void StopSFXLoop()
    {
        loopSfxSource.Stop();
    }

    public bool IsMusicPlaying(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound Not Found");
            return false;
        }
        return musicSource.isPlaying && musicSource.clip == s.clip;
    }


   public void EnableMusicLowPassFilter(bool enable)
    {
        MusicAudioLowPassFilter.enabled = enable;
        AmbientAudioLowPassFilter.enabled = enable;

    }
}
