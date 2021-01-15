using Selections;
using UnityEngine;

namespace Collectables
{
    public class Collectable : MonoBehaviour
    {
        [SerializeField] private CollectableObject collectableObject;
        private Selectable selectable;
        
        private void Start()
        {
            selectable = GetComponentInChildren<Selectable>();
            selectable.onThisSelected += Collect;
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
