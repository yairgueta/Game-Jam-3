using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace Spawners
{
    public abstract class Spawnable : MonoBehaviour
    {
        internal List<GraphNode> takenNodes;
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
