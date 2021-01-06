using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Player.Inventory
{
    public enum ResourceType
    {
        Wood, 
        Rock, 
        Mushroom,
    }
    
    [CreateAssetMenu]
    public class InventoryObject : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<ResourceType, int> quantityMap;

        private void Awake()
        {
            Setup();
        }

        public void Setup()
        {
            quantityMap = new SerializedDictionary<ResourceType, int>
            {
                {ResourceType.Wood, 0}, {ResourceType.Rock, 0}, {ResourceType.Mushroom, 10}
            };
        }

        public void AddItem(ResourceType type, int count)
        {
            quantityMap[type] += count;
            // OnInventoryChange?.Invoke(collectable.CollectableType, collectablesQuantityMap[collectable.CollectableType]);
        }

        public bool ConsumeItem(ResourceType type, int count)
        {
            var currentCount = quantityMap[type];
            if (currentCount < count) return false;
            quantityMap[type] -= count;
            return true;
            // OnInventoryChange?.Invoke(type, collectablesQuantityMap[type]);
        }
    
        
    }
}