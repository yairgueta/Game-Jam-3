using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private Slider BGMSlider, SFXSlider;

        private void OnEnable()
        {
            BGMSlider.value = SoundController.Instance.soundSettings.BGMVolume;
            SFXSlider.value = SoundController.Instance.soundSettings.sfxVolume;
        }

        private void Start()
        {
            BGMSlider.onValueChanged.AddListener(SoundController.Instance.soundSettings.SetBGMVolume);
            SFXSlider.onValueChanged.AddListener(SoundController.Instance.soundSettings.SetSFXVolume);
        }
    }
}