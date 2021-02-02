using Sound_Effects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class Menu : MonoBehaviour
    {
        public UnityAction onClickedPlay;
        [SerializeField] private Slider BGMSlider, SFXSlider;
        private void Start()
        {
            BGMSlider.value = SoundController.Instance.soundSettings.BGMVolume;
            SFXSlider.value = SoundController.Instance.soundSettings.sfxVolume;
        }

        public void Play()
        {
            SoundController.Instance.TurnMenuMusicOff();
            onClickedPlay?.Invoke();
            gameObject.SetActive(false);
        }

        public void Quit()
        {
            Application.Quit();
        }

    }
}
