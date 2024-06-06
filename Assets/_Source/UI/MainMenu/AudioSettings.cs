using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer aMixer;

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    public void SetMasterVolume()
    {
        float volume = masterSlider.value;
        aMixer.SetFloat("master", volume);
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        aMixer.SetFloat("sfx", volume);
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        aMixer.SetFloat("music", volume);
    }

}
