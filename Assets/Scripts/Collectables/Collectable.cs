using System;
using Selections;
using UnityEngine;

namespace Collectables
{
    public class Collectable : MonoBehaviour
    {
        [SerializeField] private CollectableObject collectableObject;
        private Selectable selectable;

        private void Awake()
        {
            selectable = GetComponentInChildren<Selectable>();
        }

        private void Start()
        {
            selectable.onThisSelected += Collect;
            EnableCollecting(false);
        }

        private void Collect()
        {
            collectableObject.OnCollected();
            gameObject.SetActive(false);
        }

        public void EnableCollecting(bool b) => selectable.enabled = b;

    }
}
