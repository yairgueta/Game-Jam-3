using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private GameObject tutorial;

        private void Start()
        {
            var initVol = SoundController.Instance.soundSettings.initialVolumes;
            foreach (var slider in GetComponentsInChildren<Slider>()) slider.value = initVol;
        }

        public void Play()
        {
            SoundController.Instance.TurnMenuMusicOff();
            tutorial.SetActive(true);
            gameObject.SetActive(false);
        }

        public void Quit()
        {
            Application.Quit();
        }

    }
}
