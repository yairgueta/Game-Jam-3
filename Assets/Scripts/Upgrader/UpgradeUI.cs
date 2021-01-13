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
        [SerializeField] private Button upgradeBtn;
        [SerializeField] private TMP_Text woodAmount;
        [SerializeField] private TMP_Text rockAmount;
        [SerializeField] private TMP_Text description;
        [SerializeField] private Image image;
        [SerializeField] private double crackedPercentage = 0.2;
    
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
                return;
            }
            var upgradable = SelectionManager.Instance.CurrentSelected.GetComponent<Upgradable>();
            if (upgradable == null)
            {
                upgradePanel.SetActive(false);
                return;
            }
            upgradePanel.SetActive(true);
            SetUp(upgradable);
        }

        private void SetUp(Upgradable upgradable)
        {
            upgradeBtn.interactable = true;
            upgradeBtn.onClick.RemoveAllListeners();
            upgradeBtn.onClick.AddListener(() =>
            {
                upgradable.Upgrade();
                ClosePanel();
            });

            var upgradableObject = upgradable.GetNextGradeAttributes();
            if (upgradableObject == null)
            {
                ClosePanel();
                return;
            }
            woodAmount.text = upgradableObject.requiredWoods.ToString();
            rockAmount.text = upgradableObject.requiredRocks.ToString();
            description.text = upgradableObject.description;
            image.sprite = upgradableObject.completeSprites[upgradableObject.spriteIndex];

            woodAmount.color = Color.white;
            rockAmount.color = Color.white;

            if (inventory[ResourceType.Wood] < upgradableObject.requiredWoods)
            {
                woodAmount.color = Color.red;
                upgradeBtn.interactable = false;
            }

            if (inventory[ResourceType.Rock] < upgradableObject.requiredRocks)
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
