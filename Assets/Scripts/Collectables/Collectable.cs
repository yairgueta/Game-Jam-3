using System;
using Player;
using Selections;
using UnityEngine;

namespace Collectables
{
    [RequireComponent(typeof(Selections.Selectable))]
    public class Collectable : MonoBehaviour
    {
        [SerializeField] private CollectableObject collectableObject;
        private Selectable selectable;
        
        private void Start()
        {
            selectable = GetComponent<Selectable>();
            // selectable.onThisSelected.AddListener(Collect);
            EnableCollecting(false);
        }

        private void Collect()
        {
            collectableObject.OnCollected();
            SelectionManager.Instance.Deselect();
            Destroy(gameObject);
        }

        public void EnableCollecting(bool b)
        {
            selectable.SetInteractable(b);
        }

    }
}
