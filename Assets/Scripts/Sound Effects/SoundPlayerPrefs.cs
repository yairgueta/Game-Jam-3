using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundPlayerPrefs : MonoBehaviour
{
    [SerializeField] private SoundSettings soundSettings;
    [SerializeField] private Slider bgMusicSlider;
    [SerializeField] private Slider sfxMusicSlider;
    
    // Start is called before the first frame update
    void Start()
    {
        bgMusicSlider.value = soundSettings.bgVolume;
        sfxMusicSlider.value = soundSettings.sfxVolume;
    }

    public void ChangeSFXVolume(float value)
    {
        soundSettings.sfxVolume = value;
    }

    public void ChangeBGVolume(float value)
    {
        soundSettings.bgVolume = value;
    }
}
