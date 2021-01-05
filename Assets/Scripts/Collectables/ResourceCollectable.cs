using System;
using Player;
using UnityEngine;

namespace Collectables
{
    [RequireComponent(typeof(Collectable))]
    public class ResourceCollectable : MonoBehaviour
    {
        [SerializeField] private ResourcesType collectableType;
        [SerializeField] private int quantity;
        
        public ResourcesType CollectableType => collectableType;
        public int Quantity => quantity;

        private void Start()
        {
            GetComponent<Collectable>().onThisCollected += () => CollectablesManager.Instance.onResourceCollected?.Invoke(this);
            transform.parent = CollectablesManager.Instance.resourcesParent;
        }
    }
}