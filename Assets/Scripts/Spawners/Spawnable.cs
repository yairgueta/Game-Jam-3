using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace Spawners
{
    public abstract class Spawnable : MonoBehaviour
    {
        [Tooltip("The collider in which this object is taking space in the world")][SerializeField] internal Collider2D physicsCollider;
        
        internal List<GraphNode> takenNodes;
        internal int spawnerIndex = -1;
        private Spawner spawner;

        protected virtual void Start()
        {
            physicsCollider ??= GetComponent<Collider2D>();
        }

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
