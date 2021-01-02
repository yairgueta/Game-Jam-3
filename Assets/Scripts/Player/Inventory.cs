using System;
using Collectables;
using UnityEngine;

namespace Player
{
    public enum ResourcesType
    {
        Wood, 
        Rock, 
        Ammo,
    }
    
    public class Inventory : MonoBehaviour
    {
        private void Start()
        {
            CollectablesManager.Instance.onCollected.AddListener(CollectedResource);
        }

        private void CollectedResource(Collectable collectable)
        {
            Debug.Log("Collected " + collectable.Quantity + " " + collectable.CollectableType + "s");
        }
    }
}
