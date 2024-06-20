using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds, ambientSounds;
    public AudioSource musicSource, sfxSource, loopSfxSource, ambientSource;
    public float MinDB;
    public float MaxDB;
    public AudioMixer AudioMixer;

    [SerializeField]
    AudioLowPassFilter MusicAudioLowPassFilter;
    [SerializeField]
    AudioLowPassFilter AmbientAudioLowPassFilter;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        LoadVolume();
    }

    public void PlayMusic(string clip)
    {
        Sound s = Array.Find(musicSounds, x => x.name == clip);

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

    public void StopMusic(string clip)
    {
        Sound s = Array.Find(musicSounds, x => x.name == clip);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            musicSource.Stop();
        }
    }
    public void PlayAmbient(string clip)
    {
        Sound s = Array.Find(ambientSounds, x => x.name == clip);

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

    public void StopAmbient(string clip)
    {
        Sound s = Array.Find(ambientSounds, x => x.name == clip);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            ambientSource.Stop();
        }
    }

    public void PlaySFX(string clip)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == clip);

        if (s == null)
        {
            Debug.Log("SFX Not Found");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
       
        sfxSource.PlayOneShot(clip);
        
    }
    public void PlaySFXAtSource(AudioClip clip, AudioSource source, float pitch = 1f)
    {
        source.volume = sfxSource.volume;
        source.pitch = pitch;
        source.PlayOneShot(clip);

    }

    public void PlaySFXAtSourceOnce(AudioClip clip, AudioSource source)
    {
        if(source.clip != clip)
        {
            source.clip = clip;
            source.volume = sfxSource.volume;
            source.PlayOneShot(clip);
        }
        else if(source.clip == clip && source.isPlaying == false)
        {
            source.volume = sfxSource.volume;
            source.Play();
        }

    }

    public void PlaySFXLoop(string clip)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == clip);

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

    public bool IsMusicPlaying(string clip)
    {
        Sound s = Array.Find(musicSounds, x => x.name == clip);
        if (s == null)
        {
            Debug.Log("Sound Not Found");
            return false;
        }
        return musicSource.isPlaying && musicSource.clip == s.clip;
    }

    private void LoadVolume()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            float volume = 0f;
            if (PlayerPrefs.GetFloat("MasterVolume") == 0)
                volume = -80;
            else
                volume = Mathf.Lerp(MinDB, MaxDB, PlayerPrefs.GetFloat("MasterVolume") / 10f);

            AudioMixer.SetFloat("MasterVolume", volume);
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            float volume = 0f;
            if (PlayerPrefs.GetFloat("SFXVolume") == 0)
                volume = -80;
            else
                volume = Mathf.Lerp(MinDB, MaxDB, PlayerPrefs.GetFloat("SFXVolume") / 10f);

            AudioMixer.SetFloat("SFXVolume", volume);
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            float volume = 0f;
            if (PlayerPrefs.GetFloat("MusicVolume") == 0)
                volume = -80;
            else
                volume = Mathf.Lerp(MinDB, MaxDB, PlayerPrefs.GetFloat("MusicVolume") / 10f);

            AudioMixer.SetFloat("MusicVolume", volume);
        }
    }

    public void OnStartDay()
    {
        if (IsMusicPlaying("A_NightUI_Music"))
        {
            StopMusic("A_NightUI_Music");
        }
        if (!IsMusicPlaying("A_DayUI_Music"))
        {
            PlayMusic("A_DayUI_Music");
        }
        PlayAmbient("A_DayUI_Ambient");
    }

    public void OnStartNight()
    {
        if (IsMusicPlaying("A_DayUI_Music"))
        {
            StopMusic("A_DayUI_Music");
        }
        if (!IsMusicPlaying("A_NightUI_Music"))
        {
            PlayMusic("A_NightUI_Music");
        }
        StopAmbient("A_DayUI_Ambient");
    }

    public void EnableMusicLowPassFilter(bool enable)
    {
        MusicAudioLowPassFilter.enabled = enable;
        AmbientAudioLowPassFilter.enabled = enable;

    }

    public void SetMusicPitch(float pitch)
    {
        musicSource.pitch = pitch;
    }



}
