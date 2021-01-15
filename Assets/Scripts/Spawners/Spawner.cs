using System;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using Pathfinding.Util;
using UnityEngine;

namespace Spawners
{
    [RequireComponent(typeof(Collider2D), typeof(SpawnerRandomizer))]
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private GameObject pooledPrefab;
        [SerializeField] private int totalPoolAmount;
        [SerializeField] private int startingAmount;
        [SerializeField] private ObjectPoolType spawnerType = ObjectPoolType.Other;
        
        private SpawnerRandomizer randomizer;
        private Queue<int> queuePool;
        private Spawnable[] pooledObjects;
        
        public int CurrentPooled => totalPoolAmount - queuePool?.Count ?? 0;
        public ObjectPoolType SpawnerType => spawnerType;

        
        private void Awake()
        {
            queuePool = new Queue<int>();
            pooledObjects = new Spawnable[totalPoolAmount];
            gameObject.layer = LayerMask.NameToLayer("Spawner");
        }

        private void Start()
        {
            randomizer = GetComponent<SpawnerRandomizer>();
            for (int i = 0; i < totalPoolAmount; i++)
            {
                var p = Instantiate(pooledPrefab, transform).GetComponent<Spawnable>();
                p.Init(this);
                p.spawnerIndex = i;
                pooledObjects[i] = p;
                p.gameObject.SetActive(false);
            }

            for (int i = 0; i < startingAmount; i++) SpawnRandom();
        }

        internal void SpawnableDeath(Spawnable spawnable)
        {
            queuePool.Enqueue(spawnable.spawnerIndex);
            if (spawnable.takenNodes == null) return;
            if (AstarData.active == null) return;
            foreach (var g in spawnable.takenNodes.TakeWhile(g => g != null)) g.Walkable = true;
            ListPool<GraphNode>.Release(spawnable.takenNodes);
            spawnable.takenNodes = null;
        }

        public void SpawnRandom()
        {
            if (queuePool.Count == 0)
            {
                Debug.LogWarning(name + ": Pool spawner is Out of objects, None was spawned");
                return;
            }
            var p = pooledObjects[queuePool.Dequeue()];
            p.gameObject.SetActive(true);
            randomizer.RandomizeObjectPosition(p);
            p.takenNodes.ForEach(g => g.Walkable = false);
        }

        public void SpawnRandom(int amount)
        {
            for (int i = 0; i < amount ; i++) SpawnRandom();
        }

        public void DespawnAll()
        {
            foreach (var p in pooledObjects)
            {
                p.gameObject.SetActive(false);
            }
        }
        
        public enum ObjectPoolType
        {
            Enemy,
            Collectable,
            Other
        }
    }
}
