using System;
using System.Collections;
using System.Collections.Generic;
using Sound_Effects;
using UnityEngine;
using UnityEngine.UI;

public class SoundPlayerPrefs : MonoBehaviour
{
    [SerializeField] private SoundSettings soundSettings;
    [SerializeField] private Slider bgMusicSlider;
    [SerializeField] private Slider sfxMusicSlider;
    // private AudioSource menuAudioSource;
    private SoundController soundController;

    // private void Awake()
    // {
    //     soundController = FindObjectOfType<SoundController>();
    // }
    //
    // public void ChangeSFXVolume(float value)
    // {
    //     soundSettings.sfxVolume = value;
    // }
    //
    // public void ChangeBGVolume(float value)
    // {
    //     // menuAudioSource.volume = value;
    //     soundSettings.bgVolume = value;
    //     soundController.ChangeMenuVolume(value);
    // }
}
