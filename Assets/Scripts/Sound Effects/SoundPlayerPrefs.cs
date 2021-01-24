using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundPlayerPrefs : MonoBehaviour
{
    [SerializeField] private SoundSettings soundSettings;
    [SerializeField] private Slider bgMusicSlider;
    [SerializeField] private Slider sfxMusicSlider;
    private AudioSource menuAudioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        menuAudioSource = gameObject.GetComponent<AudioSource>();
        bgMusicSlider.value = soundSettings.bgVolume;
        sfxMusicSlider.value = soundSettings.sfxVolume;
        menuAudioSource.volume = soundSettings.bgVolume;
    }

    public void ChangeSFXVolume(float value)
    {
        soundSettings.sfxVolume = value;
    }

    public void ChangeBGVolume(float value)
    {
        menuAudioSource.volume = value;
        soundSettings.bgVolume = value;
    }
}
