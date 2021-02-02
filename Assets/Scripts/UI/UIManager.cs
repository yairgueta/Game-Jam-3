using System;
using Cycles;
using DG.Tweening;
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
        
        private TMP_Text numOfCycles;
        private Tween tween;
        [SerializeField] private TMP_Text msg;
        [SerializeField] private float duration;
        private Vector3 originalScale;


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
            numOfCycles = GetComponentInChildren<TMP_Text>();
            
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
            originalScale = msg.transform.localScale;
            msg.transform.localScale = Vector3.zero;
            
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
            numOfCycles.text = CyclesManager.Instance.DaysCount.ToString();
        }

        public void DisplayMsg(String message)
        {
            msg.text = message;
            tween?.Kill(true);
            tween = DOTween.Sequence()
                .Append(msg.transform.DOScale(originalScale, duration))
                .Append(msg.transform.DOScale(Vector3.zero, duration).SetDelay(1f));
        }
    }
}