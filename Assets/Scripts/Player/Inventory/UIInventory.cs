using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    [SerializeField] private Text woodText;
    [SerializeField] private Text rockText;
    [SerializeField] private Text mushroomText;
    
    void Start()
    {
        Inventory.Instance.OnInventoryChange += UpdateResources;
    }

    private void UpdateResources(Dictionary<ResourcesType, int> dict)
    {
        woodText.text = dict[ResourcesType.Wood].ToString();
        rockText.text = dict[ResourcesType.Rock].ToString();
        mushroomText.text = dict[ResourcesType.Mushroom].ToString();
    }
}
