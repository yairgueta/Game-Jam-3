using Events;
using UnityEngine;

namespace Collectables
{
    public abstract class CollectableObject : ScriptableObject
    {
        public GameEvent onCollected;

        public virtual void OnCollected()
        {
            onCollected?.Raise();
        }
    }
}