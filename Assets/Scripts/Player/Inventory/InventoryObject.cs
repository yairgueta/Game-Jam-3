using System;
using Events;
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
        [SerializeField] private GameEvent onChange, onOutOfResources;
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
            onChange.Raise();
        }

        public void AddItem(ResourceType type, int count)
        {
            quantityMap[type] += count;
            onChange.Raise(new ResourceArg(type, 1));
        }

        public bool ConsumeItem(ResourceType type, int count)
        {
            var currentCount = quantityMap[type];
            if (currentCount < count)
            {
                onOutOfResources.Raise(new ResourceArg(type));
                return false;
            }
            quantityMap[type] -= count;
            onChange.Raise(new ResourceArg(type, -1));
            return true;
        }

        public int this[ResourceType t] => quantityMap[t];

        #region Event Arguments Class
        public class ResourceArg : EventArgs
        {
            public readonly ResourceType type;
            public readonly int isIncreasing;

            public ResourceArg(ResourceType type)
            {
                this.type = type;
                isIncreasing = 0;
            }

            public ResourceArg(ResourceType type, int isIncreasing)
            {
                this.type = type;
                this.isIncreasing = Mathf.Clamp(isIncreasing, -1, 1);
            }
        }
        #endregion
    }
}