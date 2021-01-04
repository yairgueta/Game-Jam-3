using System;
using System.Collections.Generic;
using Collectables;
using UnityEngine;
using UnityEngine.Events;

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
        public Action<ResourcesType, int> OnInventoryChange;

        protected override void Awake()
        {
            base.Awake();
            collectablesQuantityMap = new Dictionary<ResourcesType, int>
            {
                {ResourcesType.Wood, 0}, {ResourcesType.Rock, 0}, {ResourcesType.Mushroom, 0}
            };
        }

        private void Start()
        {
            CollectablesManager.Instance.onResourceCollected += CollectedResource;
        }

        private void CollectedResource(ResourceCollectable collectable)
        {
            collectablesQuantityMap[collectable.CollectableType] += collectable.Quantity;
            OnInventoryChange?.Invoke(collectable.CollectableType, collectablesQuantityMap[collectable.CollectableType]);
        }

        private void UseResource(ResourcesType type, int quantity)
        {
            collectablesQuantityMap[type] -= quantity;
            OnInventoryChange?.Invoke(type, collectablesQuantityMap[type]);
        }
        
    }
}
