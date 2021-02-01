using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class Menu : MonoBehaviour
    {
        public UnityAction onClickedPlay;
        private void Start()
        {
            var sliders = GetComponentsInChildren<Slider>();
            sliders[0].value = SoundController.Instance.soundSettings.BGMVolume;
            sliders[1].value = SoundController.Instance.soundSettings.sfxVolume;
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
