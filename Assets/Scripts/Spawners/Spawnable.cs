using UnityEngine;

namespace Spawners
{
    public abstract class Spawnable : MonoBehaviour
    {

        private Spawner spawner;
        
        internal void Init(Spawner spawnerParent)
        {
            this.spawner = spawnerParent;
        }

        protected virtual void OnDisable()
        {
            spawner?.SpawnableDeath(this);
        }
    }
}
