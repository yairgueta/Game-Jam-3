using System;
using System.Collections.Generic;
using Player;
using Player.Inventory;
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
        }

        private void UpdateResources(ResourceType t, int q)
        {
            switch (t)
            {
                case ResourceType.Wood:
                    woodText.text = q.ToString();
                    break;
                case ResourceType.Rock:
                    rockText.text = q.ToString();
                    break;
                case ResourceType.Mushroom:
                    mushroomText.text = q.ToString();
                    break;
            }
        }
    }
}
