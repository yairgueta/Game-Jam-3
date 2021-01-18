using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Events;
using Player.Inventory;
using Selections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Upgrader;

namespace UI
{
    public class UpgradeUI : MonoBehaviour
    {
        private static InventoryObject inventory => Player.PlayerController.CurrentInventory;
        [SerializeField] private GameObject upgradePanel;
        [SerializeField] private GameEvent onNewSelection;
        [SerializeField] private Button btn;
        [SerializeField] private TMP_Text woodAmount;
        [SerializeField] private TMP_Text rockAmount;
        private Tween raiseAnimationTween;
        private TMP_Text btnText;

        private void Start()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;
            
            var
            
            btnText = btn.GetComponentInChildren<TMP_Text>();
            var listener = gameObject.AddComponent<GameEventListener>();
            listener.InitEvent(onNewSelection);
            listener.response.AddListener(o => OnNewSelection_Response());
            ClosePanel();
        }

        private void OnNewSelection_Response()
        {
            if (SelectionManager.Instance.CurrentSelected == null) 
            {
                upgradePanel.SetActive(false);
                return;
            }
            
            var fixable = SelectionManager.Instance.CurrentSelected.GetComponentInChildren<Fixable>();
            if (fixable != null)
            {
                if (fixable.ShouldFix)
                {
                    SetUpPanel(fixable.Fix, fixable.RequiredWood, fixable.RequiredRock,"FIX");
                    return;
                }
            }
            
            var upgradable = SelectionManager.Instance.CurrentSelected.GetComponentInParent<Upgradable>();
            if (upgradable == null)
            {
                upgradePanel.SetActive(false);
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
            btn.interactable = false;
            woodAmount.text = "-";
            rockAmount.text = "-";
            btnText.text = "Max Level!";
        }

        private void SetUpTexts(int woods, int rocks)
        {
            woodAmount.text = woods.ToString();
            rockAmount.text = rocks.ToString();

            woodAmount.color = Color.black;
            rockAmount.color = Color.black;

            if (inventory[ResourceType.Wood] < woods)
            {
                woodAmount.color = Color.red;
                btn.interactable = false;
            }

            if (inventory[ResourceType.Rock] < rocks)
            {
                rockAmount.color = Color.red;
                btn.interactable = false;
            }
        }
        
        private void RaiseWindow()
        {
            transform.position = SelectionManager.Instance.CurrentSelected.transform.position;
            upgradePanel.SetActive(true);
            
            raiseAnimationTween?.Kill(true);
            upgradePanel.transform.localScale = Vector3.zero;
            raiseAnimationTween = upgradePanel.transform.DOScale(Vector3.one, .7f).SetEase(Ease.OutBounce);
        }

        private void ClosePanel()
        {
            raiseAnimationTween?.Kill(true);
            raiseAnimationTween = upgradePanel.transform.DOScale(Vector3.zero, .3f).OnComplete(() => SelectionManager.Instance.Deselect());
        }
    }
}
