using Events;
using UnityEngine;

namespace Collectables
{
    [CreateAssetMenu(menuName = "Collectables/Other")]
    public class CollectableObject : ScriptableObject
    {
        public GameEvent onCollected;

        public virtual void OnCollected()
        {
            onCollected?.Raise();
        }
    }
}