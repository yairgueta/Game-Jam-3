using System;
using UnityEngine;

namespace Collectables
{
    [RequireComponent(typeof(Collectable))]
    public class HealthFlower : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Collectable>().onThisCollected += () => CollectFlower();
            transform.parent = CollectablesManager.Instance.flowersParent;
        }

        private void CollectFlower()
        {
            CollectablesManager.Instance.onHealthFlowerCollected?.Invoke(this);
        }
    }
}