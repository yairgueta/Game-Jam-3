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
            var initVol = SoundController.Instance.soundSettings.initialVolumes;
            foreach (var slider in GetComponentsInChildren<Slider>()) slider.value = initVol;
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
