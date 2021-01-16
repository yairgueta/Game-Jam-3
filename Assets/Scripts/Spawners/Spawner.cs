using System;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using Pathfinding.Util;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Spawners
{
    [RequireComponent(typeof(Collider2D), typeof(SpawnerRandomizer))]
    public class Spawner : MonoBehaviour
    {
        [HideInInspector][SerializeField] private GameObject[] pooledPrefab;
        
        [SerializeField] private int totalPoolAmount;
        [SerializeField] private ObjectPoolType spawnerType = ObjectPoolType.Other;
        [HideInInspector][SerializeField] private int startingAmount;
        [HideInInspector][SerializeField] private bool usePerlinNoise;
        [HideInInspector][SerializeField] private float[] percentageToSpawn;
        
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

        private GameObject GetRandomPrefab()
        {
            var ran = Random.Range(0f, 1f);
            var total = 0f;
            for (var i = 0; i< pooledPrefab.Length; i++)
            {
                if (ran >= total && ran < total + percentageToSpawn[i]) return pooledPrefab[i];
                total += percentageToSpawn[i];
            }
            return null;
        }
        
        private void Start()
        {
            randomizer = GetComponent<SpawnerRandomizer>();
            for (int i = 0; i < totalPoolAmount; i++)
            {
                var p = Instantiate(GetRandomPrefab(), transform).GetComponent<Spawnable>();
                p.Init(this);
                p.spawnerIndex = i;
                pooledObjects[i] = p;
                p.gameObject.SetActive(false);
            }

            if (usePerlinNoise)
                SpawnNoise(startingAmount);
            else
                SpawnRandom(startingAmount);
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

        private void SpawnRandom_Method(Action<Spawnable> randomMethod)
        {
            if (queuePool.Count == 0)
            {
                Debug.LogWarning(name + ": Pool spawner is Out of objects, None was spawned");
                return;
            }
            var p = pooledObjects[queuePool.Dequeue()];
            p.gameObject.SetActive(true);
            randomMethod(p);
            p.takenNodes.ForEach(g => g.Walkable = false);
        }
        
        public void SpawnRandom(int amount)
        {
            for (int i = 0; i < amount ; i++) SpawnRandom_Method(randomizer.RandomizeObjectPosition);
        }

        public void SpawnNoise(int amount)
        {
            for (int i = 0; i < amount; i++) SpawnRandom_Method(randomizer.PerlinRandomizeObjectPosition);
        }

        public void DespawnAll()
        {
            foreach (var p in pooledObjects)
                p.gameObject.SetActive(false);
        }

        public void Spawn(int n)
        {
            if (usePerlinNoise) SpawnNoise(n);
            else SpawnRandom(n);
        }

        public void SpawnAll()
        {
            Spawn(totalPoolAmount - CurrentPooled);
        }
        public enum ObjectPoolType
        {
            Enemy,
            Collectable,
            Other
        }
    }
}
