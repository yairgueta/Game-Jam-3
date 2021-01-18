using System;
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
    
        [SerializeField] private GameEvent onNewSelection;
        [SerializeField] private Button btn;
        [SerializeField] private TMP_Text woodAmount;
        [SerializeField] private TMP_Text rockAmount;
        private void Start()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;

            var listener = gameObject.AddComponent<GameEventListener>();
            listener.InitEvent(onNewSelection);
            listener.response.AddListener(o => OnNewSelection_Response());
        }

        private void OnNewSelection_Response()
        {
            if (SelectionManager.Instance.CurrentSelected == null) 
            {
            Debug.Log("hi");
                gameObject.SetActive(false);
                return;
            }
            
            var fixable = SelectionManager.Instance.CurrentSelected.GetComponentInChildren<Fixable>();
            if (fixable != null)
            {
                if (fixable.ShouldFix)
                {
                    SetUpPanel(fixable.Fix, fixable.RequiredWood, fixable.RequiredRock);
                    return;
                }
            }
            
            var upgradable = SelectionManager.Instance.CurrentSelected.GetComponentInParent<Upgradable>();
            if (upgradable == null)
            {
                gameObject.SetActive(false);
                return;
            }

            SetUpPanel(upgradable.Upgrade, upgradable.NextGradeRequiredWood, upgradable.NextGradeRequiredRock);
        }

        
        private void SetUpPanel(Action onClick, int woodReq, int rockReq)
        {
            gameObject.SetActive(true);
            btn.interactable = true;
            btn.onClick.RemoveAllListeners();

            void btnOnClick()
            {
                onClick();
                btn.onClick.RemoveListener(btnOnClick);
                ClosePanel();
            }
            
            btn.onClick.AddListener(btnOnClick);
            
            SetUpTexts(woodReq, rockReq);
        }
        

        private void SetUpTexts(int woods, int rocks)
        {
            woodAmount.text = woods.ToString();
            rockAmount.text = rocks.ToString();

            woodAmount.color = Color.white;
            rockAmount.color = Color.white;

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
        
        public void ClosePanel()
        {
            SelectionManager.Instance.Deselect();
        }
    }
}
