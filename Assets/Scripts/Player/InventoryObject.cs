using System;
using System.Collections.Generic;
using Events;
using UnityEngine;
using UnityEngine.Rendering;

namespace Player
{
    public enum ResourceType
    {
        Wood = 0, 
        Rock = 1, 
    }

    [CreateAssetMenu]
    public class InventoryObject : ScriptableObject
    {
        [Header("Initial Quantities")] [SerializeField]
        private int initWood;

        [SerializeField] private int initRock;

        [Header("Inventory Things")] [SerializeField]
        private GameEvent onChange;

        [SerializeField] private GameEvent onOutOfResources;
        [SerializeField] private List<int> quantityMap;

        public void Setup()
        {
            quantityMap = new List<int> {initWood, initRock};
            onChange.Raise();
        }

        public int this[ResourceType t]
        {
            get => quantityMap[(int) t];
            set
            {
                if (value < 0)
                {
                    onOutOfResources.Raise(new CollectingArgs(t));
                    throw new InventoryOutOfResourceException();
                }

                var difference = value - quantityMap[(int) t];
                quantityMap[(int) t] = value;
                onChange.Raise(new CollectingArgs(t, difference, difference));
            }
        }

        #region Classes

        public class InventoryOutOfResourceException : Exception
        {
        }

        public class CollectingArgs
        {
            public readonly ResourceType type;
            public readonly int isIncreasing;
            public readonly int difference;

            public CollectingArgs(ResourceType type)
            {
                this.type = type;
                isIncreasing = 0;
                difference = 0;
            }

            public CollectingArgs(ResourceType type, int isIncreasing, int diff)
            {
                this.type = type;
                this.isIncreasing = Mathf.Clamp(isIncreasing, -1, 1);
                difference = diff;
            }
        }

        #endregion
    }
}