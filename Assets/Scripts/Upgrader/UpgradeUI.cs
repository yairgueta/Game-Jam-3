using System;
using Events;
using Player;
using Player.Inventory;
using Selections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Upgrader
{
    public class UpgradeUI : MonoBehaviour
    {
        [SerializeField] private GameEvent onNewSelection;
        [SerializeField] private InventoryObject inventory;
    
        [SerializeField] private GameObject upgradePanel;
        [SerializeField] private GameObject maxGradePanel;
        [SerializeField] private Button upgradeBtn;
        [SerializeField] private Button fixBtn;
        [SerializeField] private TMP_Text woodAmount;
        [SerializeField] private TMP_Text rockAmount;
        [SerializeField] private TMP_Text description;
        [SerializeField] private Image image;
        [SerializeField] private String fixDescription = "would you like to fix?";
        private void Start()
        {
            var listener = gameObject.AddComponent<GameEventListener>();
            listener.InitEvent(onNewSelection);
            listener.response.AddListener(o => OnNewSelection_Response());
        }

        private void OnNewSelection_Response()
        {
            if (SelectionManager.Instance.CurrentSelected == null) 
            {
                upgradePanel.SetActive(false);
                maxGradePanel.SetActive(false);
                return;
            }
            
            var fixable = SelectionManager.Instance.CurrentSelected.GetComponentInChildren<Fixable>();
            Debug.Log(fixable);
            if (fixable != null)
            {
                if (fixable.ShouldFix())
                {
                    Fixing(fixable);
                    return;
                }
            }
            
            var upgradable = SelectionManager.Instance.CurrentSelected.GetComponentInParent<Upgradable>();
            if (upgradable == null)
            {
                upgradePanel.SetActive(false);
                return;
            }

            Upgrading(upgradable);
        }

        private void Fixing(Fixable fixable)
        {
            upgradePanel.SetActive(true);
            upgradeBtn.gameObject.SetActive(false);
            fixBtn.gameObject.SetActive(true);
            
            fixBtn.interactable = true;
            fixBtn.onClick.RemoveAllListeners();
            fixBtn.onClick.AddListener(() =>
            {
                fixable.Fix();
                ClosePanel();
            });

            var requiredWoods = fixable.RequiredWoodsPercentage();
            var requiredRocks = fixable.RequiredRocksPercentage();
            
            SetUpTexts(requiredWoods, requiredRocks, fixDescription, fixable.GetCompleteSprite(), false);
        }

        
        private void Upgrading(Upgradable upgradable)
        {
            var upgradableObject = upgradable.GetNextGradeAttributes();
            
            if (upgradableObject == null)
            {
                maxGradePanel.SetActive(true);
                return;
            }
            upgradePanel.SetActive(true);
            SetUp(upgradable);
        }

        private void SetUp(Upgradable upgradable)
        {
            upgradeBtn.gameObject.SetActive(true);
            fixBtn.gameObject.SetActive(false);
            
            upgradeBtn.interactable = true;
            upgradeBtn.onClick.RemoveAllListeners();
            upgradeBtn.onClick.AddListener(() =>
            {
                upgradable.Upgrade();
                ClosePanel();
            });

            var upgradableObject = upgradable.GetNextGradeAttributes();
            
            SetUpTexts(upgradableObject.requiredWoods, upgradableObject.requiredRocks,upgradableObject.description, 
                upgradableObject.completeSprites[upgradableObject.spriteIndex], true);
        }


        private void SetUpTexts(int woods, int rocks, String desc, Sprite sprite, bool withImg)
        {
            woodAmount.text = woods.ToString();
            rockAmount.text = rocks.ToString();
            description.text = desc;
            if (withImg)
            {
                image.gameObject.SetActive(true);
                image.sprite = sprite;
            }
            else
            {
                image.gameObject.SetActive(false);
            }

            woodAmount.color = Color.white;
            rockAmount.color = Color.white;

            if (inventory[ResourceType.Wood] < woods)
            {
                woodAmount.color = Color.red;
                upgradeBtn.interactable = false;
            }

            if (inventory[ResourceType.Rock] < rocks)
            {
                rockAmount.color = Color.red;
                upgradeBtn.interactable = false;
            }
        }

 

        public void ClosePanel()
        {
            SelectionManager.Instance.Deselect();
        }
    }
}
