using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;


    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            LoadVolume();
        }
        else
        { 
            SetMasterVolume();
            SetMusicVolume();
            SetSFXVolume();
        }
    }

    public void SetMasterVolume()
    {
        float volume = 0f;
        if (masterSlider.value == 0)
            volume = -80;
        else
            volume = Mathf.Lerp(AudioManager.Instance.MinDB, AudioManager.Instance.MaxDB, masterSlider.value / 10f);
        AudioManager.Instance.AudioMixer.SetFloat("MasterVolume", volume);
        PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
    }

    public void SetSFXVolume()
    {
        float volume = 0f;
        if (sfxSlider.value == 0)
            volume = -80;
        else
            volume = Mathf.Lerp(AudioManager.Instance.MinDB, AudioManager.Instance.MaxDB, sfxSlider.value / 10f);
        AudioManager.Instance.AudioMixer.SetFloat("SFXVolume", volume);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
    }

    public void SetMusicVolume()
    {
        float volume = 0f;
        if (musicSlider.value == 0)
            volume = -80;
        else
            volume = Mathf.Lerp(AudioManager.Instance.MinDB, AudioManager.Instance.MaxDB, musicSlider.value / 10f);

        AudioManager.Instance.AudioMixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
    }

    private void LoadVolume()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        }
    }
}
