using System;
using Events;
using Player;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI Refernces")] [SerializeField]
        private GameObject mainMenu;

        [SerializeField] private GameObject tutorial;
        [SerializeField] private GameObject mainUI;
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject deathWindow;
        // [SerializeField] private TMP_Text numOfCycles;
        [SerializeField] private TMP_Text numOfWoods;
        [SerializeField] private TMP_Text numOfRocks;


        [Header("UI Triggers")] [SerializeField]
        private GameEvent triggerBlur;

        [SerializeField] private GameEvent triggerUnblur;


        private void OnEnable()
        {
            GameManager.Instance.UIManagerInstance = this;
        }

        private void OnDisable()
        {
            GameManager.Instance.UIManagerInstance = null;
        }


        private void Start()
        {
            mainMenu.SetActive(true);
            tutorial.SetActive(false);
            mainUI.SetActive(false);
            pauseMenu.SetActive(false);
            triggerBlur.Raise();

            mainMenu.GetComponent<Menu>().onClickedPlay += () =>
            {
                mainMenu.SetActive(false);
                tutorial.SetActive(true);
            };
            tutorial.GetComponent<TutorialManager>().onClickedStart += () =>
            {
                tutorial.SetActive(false);
                mainUI.SetActive(true);
                triggerUnblur.Raise();
                GameManager.Instance.StartGame();
            };

            pauseMenu.GetComponent<PauseMenu>().InitReferences(GameManager.Instance.RestartGame, SetPauseMenu);
        }

        public void SetPauseMenu()
        {
            if (!GameManager.Instance.IsPlaying) return;
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            GameEvent trigger = pauseMenu.activeSelf ? triggerBlur : triggerUnblur;
            mainUI.SetActive(!mainUI.activeSelf);
            Time.timeScale = pauseMenu.activeSelf ? 0 : 1;
            trigger.Raise();
        }

        public void RaiseDeathWindow()
        {
            mainUI.SetActive(false);
            triggerBlur.Raise();
            deathWindow.SetActive(true);
            // numOfCycles.text = cyclesNum.ToString();
            numOfRocks.text = PlayerController.CurrentInventory.GetFinalRocks.ToString();
            numOfWoods.text = PlayerController.CurrentInventory.GetFinalWoods.ToString();
        }
    }
}