using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace Spawners
{
    public class Spawnable : MonoBehaviour
    {
        [Tooltip("The collider in which this object is taking space in the world")][SerializeField] internal Collider2D physicsCollider;
        [SerializeField] private float radius = .5f;
        public float Radius => radius;
        
        internal List<GraphNode> takenNodes;
        internal int spawnerIndex = -1;
        private Action<Spawnable> onThisDeath;
        
        private void Awake()
        {
            if(!physicsCollider) physicsCollider = GetComponent<Collider2D>();
        }

        internal void Init(Action<Spawnable> deathAction)
        {
            onThisDeath = deathAction;
        }

        protected virtual void OnDisable()
        {
            onThisDeath?.Invoke(this);
        }
    }
}
