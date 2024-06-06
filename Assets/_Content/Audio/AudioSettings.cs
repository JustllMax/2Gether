using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer aMixer;

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    private void Start()
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
        float volume = Mathf.Lerp(-38f, 5f, masterSlider.value / 10f);
        aMixer.SetFloat("MasterVolume", volume);
        PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
    }

    public void SetSFXVolume()
    {
        float volume = Mathf.Lerp(-38f, 10f, sfxSlider.value / 10f);
        aMixer.SetFloat("SFXVolume", volume);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
    }

    public void SetMusicVolume()
    {
        float volume = Mathf.Lerp(-38f, 5f, musicSlider.value / 10f);
        aMixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
    }

    private void LoadVolume()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
            SetMasterVolume();
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
            SetSFXVolume();
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
            SetMusicVolume();
        }
    }
}
