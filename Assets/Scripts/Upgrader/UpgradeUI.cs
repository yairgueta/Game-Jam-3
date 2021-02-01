using System;
using System.Collections.Generic;
using DG.Tweening;
using Events;
using Player;
using Selections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Selectable = Selections.Selectable;

namespace Upgrader
{
    public class UpgradeUI : MonoBehaviour
    {
        private static InventoryObject inventory => PlayerController.CurrentInventory;
        
        [Header("References")]
        [SerializeField] private GameObject upgradePanel;
        [SerializeField] private GameEvent onNewSelection;
        [SerializeField] private Button btn;
        [SerializeField] private GameObject woodGO;
        [SerializeField] private GameObject rockGO;
        
        [Header("Animation Attributes")]
        [SerializeField] private float openDuration;
        [SerializeField] private Ease openEase;
        [SerializeField] private float closeDuration;
        [SerializeField] private Ease closeEase;

        
        private Selectable previousWindowRaised;
        private Tween raiseAnimationTween;
        private TMP_Text btnText;
        
        private TMP_Text woodAmount;
        private TMP_Text rockAmount;
        
        private void Start()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;
            btnText = btn.GetComponentInChildren<TMP_Text>();

            woodAmount = woodGO.GetComponentInChildren<TMP_Text>();
            rockAmount = rockGO.GetComponentInChildren<TMP_Text>();
            
            var listener = gameObject.AddComponent<GameEventListener>();
            listener.InitEvent(onNewSelection);
            listener.response.AddListener(o => OnNewSelection_Response());
            ClosePanel();
        }

        private void OnNewSelection_Response()
        {
            if (!MouseInputHandler.Instance.currentSelected) 
            {
                ClosePanel();
                return;
            }

            if (MouseInputHandler.Instance.currentSelected == previousWindowRaised) return;  // Do nothing when selected the same!

            var fixable = MouseInputHandler.Instance.currentSelected.GetComponentInParent<Fixable>();
            if (fixable != null)
            {

                void FixableHealthChange()
                {
                    ClosePanel();
                    fixable.onHealthChange -= FixableHealthChange;
                }

                fixable.onHealthChange += FixableHealthChange;
                if (fixable.ShouldFix)
                {
                    SetUpPanel(fixable.Fix, fixable.RequiredWood, fixable.RequiredRock,"FIX");
                    return;
                }
            }
            
            var upgradable = MouseInputHandler.Instance.currentSelected.GetComponentInParent<Upgradable>();
            if (upgradable == null)
            {
                ClosePanel();
                return;
            }

            if (upgradable.IsMaxGrade)
            {
                SetUpMaxPanel();
            }
            else SetUpPanel(upgradable.Upgrade, upgradable.NextGradeRequiredWood, upgradable.NextGradeRequiredRock, "UPGRADE");
        }


        private void SetUpPanel(Action onClick, int woodReq, int rockReq, string btnString)
        {
            RaiseWindow();
            btn.interactable = true;
            btn.onClick.RemoveAllListeners();
            
            // costsImages.ForEach(c => c.SetActive(true));
            
            void BtnOnClick()
            {
                onClick();
                btn.onClick.RemoveListener(BtnOnClick);
                ClosePanel();
            }
            
            btn.onClick.AddListener(BtnOnClick);
            btnText.text = btnString;
            SetUpTexts(woodReq, rockReq);
        }
        
        private void SetUpMaxPanel()
        {
            RaiseWindow();
            // costsImages.ForEach(c => c.SetActive(false));
            btn.interactable = false;
            woodGO.SetActive(false);
            rockGO.SetActive(false);
            btnText.text = "Max Level!";
        }

        private void SetUpTexts(int woods, int rocks)
        {
            woodAmount.text = woods.ToString();
            rockAmount.text = rocks.ToString();

            woodAmount.color = Color.black;
            rockAmount.color = Color.black;

            bool outOfResources = false;
            if (inventory[ResourceType.Wood] < woods)
            {
                woodAmount.color = Color.red;
                btn.interactable = false;
                outOfResources = true;
            }

            if (inventory[ResourceType.Rock] < rocks)
            {
                rockAmount.color = Color.red;
                btn.interactable = false;
                outOfResources = true;
            }

            if (outOfResources)
            {
                // GameManager.Instance.DisplayMsg("out of resources");
            }
        }

        
        private void RaiseWindow()
        {
            rockGO.SetActive(true);
            woodGO.SetActive(true);
            
            previousWindowRaised = MouseInputHandler.Instance.currentSelected;
            transform.position = previousWindowRaised.transform.position;
            upgradePanel.SetActive(true);
            
            raiseAnimationTween?.Kill(true);
            upgradePanel.transform.localScale = Vector3.zero;
            raiseAnimationTween = upgradePanel.transform.DOScale(Vector3.one, openDuration).SetEase(openEase);
        }

        private void ClosePanel()
        {
            previousWindowRaised = null;
            raiseAnimationTween?.Kill(true);
            raiseAnimationTween = upgradePanel.transform.DOScale(Vector3.zero, closeDuration).SetEase(closeEase).OnComplete(() => MouseInputHandler.Instance.Deselect());
        }
    }
}
