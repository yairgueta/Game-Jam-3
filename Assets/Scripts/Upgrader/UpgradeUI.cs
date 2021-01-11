using Events;
using Player.Inventory;
using Selections;
using TMPro;
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
    
        private void Start()
        {
            var listener = gameObject.AddComponent<GameEventListener>();
            listener.InitEvent(onNewSelection);
            listener.response.AddListener(o => OnNewSelection_Response());
        }

        private void OnNewSelection_Response()
        {
            Debug.Log("miri");
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
            Debug.Log("Set up" );
            Debug.Log(SelectionManager.Instance.CurrentSelected);
            upgradeBtn.interactable = true;
            upgradeBtn.onClick.RemoveAllListeners();
            upgradeBtn.onClick.AddListener(() =>
            {
                upgradable.Upgrade();
                ClosePanel();
            });

            var upgradableObject = upgradable.GetNextGradeAttributes();
            woodAmount.text = upgradableObject.requiredWoods.ToString();
            rockAmount.text = upgradableObject.requiredRocks.ToString();
            description.text = upgradableObject.description;
            image.sprite = upgradableObject.sprite;

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
            Debug.Log("closed");
            SelectionManager.Instance.Deselect();
            Debug.Log(SelectionManager.Instance.CurrentSelected);
        }
    }
}
