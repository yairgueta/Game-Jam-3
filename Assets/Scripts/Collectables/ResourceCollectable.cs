using System;
using Player;
using Player.Inventory;
using UnityEngine;

namespace Collectables
{
    [RequireComponent(typeof(Collectable))]
    public class ResourceCollectable : MonoBehaviour
    {
        [SerializeField] private ResourceType collectableType;
        [SerializeField] private int quantity;
        
        public ResourceType CollectableType => collectableType;
        public int Quantity => quantity;

        private void Start()
        {
            GetComponent<Collectable>().onThisCollected += () => CollectablesManager.Instance.onResourceCollected?.Invoke(this);
            transform.parent = CollectablesManager.Instance.resourcesParent;
        }
    }
}