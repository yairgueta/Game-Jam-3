using System;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIInventory : MonoBehaviour
    {
        [SerializeField] private Text woodText;
        [SerializeField] private Text rockText;
        [SerializeField] private Text mushroomText;
    
        void Start()
        {
            Inventory.Instance.OnInventoryChange += UpdateResources;
        }

        private void UpdateResources(ResourcesType t, int q)
        {
            switch (t)
            {
                case ResourcesType.Wood:
                    woodText.text = q.ToString();
                    break;
                case ResourcesType.Rock:
                    rockText.text = q.ToString();
                    break;
                case ResourcesType.Mushroom:
                    mushroomText.text = q.ToString();
                    break;
            }
        }
    }
}
