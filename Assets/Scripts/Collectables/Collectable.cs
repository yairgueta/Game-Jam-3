using System;
using Player;
using Selections;
using UnityEngine;

namespace Collectables
{
    [RequireComponent(typeof(Selections.Selectable))]
    public class Collectable : MonoBehaviour
    {
        internal static Action<Collectable> OnCollected;
        [SerializeField] private ResourcesType collectableType;
        [SerializeField] private int quantity;
        
        private Selectable selectable;

        public ResourcesType CollectableType => collectableType;
        public int Quantity => quantity;

        private void Start()
        {
            selectable = GetComponent<Selectable>();
            selectable.onThisSelected.AddListener(Collect);
            EnableCollecting(false);
        }

        private void Collect()
        {
            OnCollected?.Invoke(this);
            // TODO: Collection animation / particle?
            
            selectable.Deselect();
            Destroy(gameObject);
        }

        public void EnableCollecting(bool b)
        {
            selectable.SetInteractable(b);
        }

    }
}
