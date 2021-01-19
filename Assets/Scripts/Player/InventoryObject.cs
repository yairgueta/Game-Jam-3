using System;
using Events;
using UnityEngine;
using UnityEngine.Rendering;

namespace Player
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
        [Header("Initial Quantities")] 
        [SerializeField] private int initWood;
        [SerializeField] private int initRock;

        [Header("Inventory Things")]
        [SerializeField] private GameEvent onChange;
        [SerializeField] private GameEvent onOutOfResources;
        [SerializeField] private SerializedDictionary<ResourceType, int> quantityMap;
        
        

        public void Setup()
        {
            quantityMap = new SerializedDictionary<ResourceType, int>
            {
                {ResourceType.Wood, initWood}, {ResourceType.Rock, initRock}, {ResourceType.Mushroom, 0}
            };
            onChange.Raise();
        }

        public int this[ResourceType t]
        {
            get => quantityMap[t];
            set
            {
                if (value < 0)
                {
                    onOutOfResources.Raise(new CollectingArgs(t));
                    throw new InventoryOutOfResourceException();
                }
                var difference = value - quantityMap[t];
                quantityMap[t] = value;
                onChange.Raise(new CollectingArgs(t, difference));
            }
        } 

        #region Classes

        public class InventoryOutOfResourceException : Exception { }
        
        public class CollectingArgs
        {
            public readonly ResourceType type;
            public readonly int isIncreasing;

            public CollectingArgs(ResourceType type)
            {
                this.type = type;
                isIncreasing = 0;
            }

            public CollectingArgs(ResourceType type, int isIncreasing)
            {
                this.type = type;
                this.isIncreasing = Mathf.Clamp(isIncreasing, -1, 1);
            }
        }
        #endregion
    }
}