using System;
using System.Collections.Generic;
using Collectables;
using UnityEngine;

namespace Player
{
    public enum ResourcesType
    {
        Wood, 
        Rock, 
        Mushroom,
    }
    
    public class Inventory : Singleton<Inventory>
    {
        private Dictionary<ResourcesType, int> collectablesQuantityMap;
        public event Action<Dictionary<ResourcesType, int>> OnInventoryChange; 

        private void Start()
        {
            CollectablesManager.Instance.onCollected.AddListener(CollectedResource);
            collectablesQuantityMap = new Dictionary<ResourcesType, int>()
            {
                {ResourcesType.Wood, 0}, {ResourcesType.Rock, 0}, {ResourcesType.Mushroom, 0}
            };
        }

        private void CollectedResource(Collectable collectable)
        {
            Debug.Log("Collected " + collectable.Quantity + " " + collectable.CollectableType + "s");
            collectablesQuantityMap[collectable.CollectableType] += collectable.Quantity;
            OnInventoryChange?.Invoke(collectablesQuantityMap);
        }

        private void UseResource(ResourcesType type, int quantity)
        {
            collectablesQuantityMap[type] -= quantity;
            OnInventoryChange?.Invoke(collectablesQuantityMap);

        }
        
    }
}
