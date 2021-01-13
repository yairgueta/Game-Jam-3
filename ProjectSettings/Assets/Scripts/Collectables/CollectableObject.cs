using UnityEngine;
using UnityEngine.Events;

namespace Collectables
{
    public abstract class CollectableObject : ScriptableObject
    {
        public UnityEvent onCollectedUnityEvent;

        public virtual void OnCollected()
        {
            onCollectedUnityEvent?.Invoke();
        }
    }
}