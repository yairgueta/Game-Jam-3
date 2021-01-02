using UnityEngine;
using UnityEngine.Events;

namespace Collectables
{
    public class CollectablesManager : Singleton<CollectablesManager>
    {
        public UnityEvent<Collectable> onCollected;
        
        void Start()
        {
            Collectable.OnCollected += c => onCollected?.Invoke(c);
        }
    }
}
