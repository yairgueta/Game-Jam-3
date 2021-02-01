using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        [SerializeField] private GameObject[] pooledPrefabs;
        [SerializeField] private int maxAmount;
        [SerializeField] private bool useOverFrameSpawning = true;
        [SerializeField] private float overFrameTimeout = .5f;
        [SerializeField] private int startingAmount;
        [SerializeField] private bool usePerlinNoise;
        [SerializeField] private float[] percentageToSpawn;

        private ObjectPool[] objectPools;
        private HashSet<Spawnable> currentSpawned; 
        private SpawnerRandomizer randomizer;
        private Action gameManagerWaitingListOnFinishTask;

        private void Awake()
        {
            currentSpawned = new HashSet<Spawnable>();
            randomizer = GetComponent<SpawnerRandomizer>();
            gameObject.layer = LayerMask.NameToLayer("Spawner");
        }

        private void Start()
        {
            objectPools = new ObjectPool[pooledPrefabs.Length];
            for (int i = 0; i < pooledPrefabs.Length; i++)
                objectPools[i] = ObjectPoolManager.Instance.GetObjectPool(pooledPrefabs[i]);
            
            Spawn(startingAmount, useOverFrameSpawning);
            if (useOverFrameSpawning)
                gameManagerWaitingListOnFinishTask = GameManager.Instance.RegisterToWaitingList();
        }

        private Spawnable PoolRandomPrefab()
        {
            var ran = Random.Range(0f, 1f);
            var total = 0f;
            for (var i = 0; i < pooledPrefabs.Length; i++)
            {
                if (ran >= total && ran < total + percentageToSpawn[i])
                {
                    var p = objectPools[i].Pool();
                    currentSpawned.Add(p);
                    p.onThisDeath += SpawnableDeath;
                    return p;
                }
                total += percentageToSpawn[i];
            }
            return null;
        }

        private void OnDisable()
        {
            var list = currentSpawned.ToList();
            for (int i = list.Count - 1; i >= 0; i--)
            {
                list[i].gameObject.SetActive(false);
            }
        }

        private void SpawnableDeath(Spawnable spawnable)
        {
            spawnable.onThisDeath -= SpawnableDeath;
            currentSpawned.Remove(spawnable);
            
            if (spawnable.takenNodes == null) return;
            if (AstarData.active == null) return;
            foreach (var g in spawnable.takenNodes.TakeWhile(g => g != null)) g.Walkable = true;
            ListPool<GraphNode>.Release(spawnable.takenNodes);
            spawnable.takenNodes = null;
        }

        private void SpawnRandom_Method(Action<Spawnable> randomMethod)
        {
            var p = PoolRandomPrefab();
            p.gameObject.SetActive(true);
            randomMethod.Invoke(p);
            p.takenNodes.ForEach(g => g.Walkable = false);
        }

        private IEnumerator SpawnOverFrames(int count, Action<Spawnable> randomMethod)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < count; i++)
            {
                SpawnRandom_Method(randomMethod);
                if (stopwatch.ElapsedMilliseconds < overFrameTimeout) continue;
                yield return null;
                stopwatch.Restart();
            }
            gameManagerWaitingListOnFinishTask();
        }
        
        public void SpawnRandom(int amount, bool useMultiFrame)
        {
            if (useMultiFrame)
                StartCoroutine(SpawnOverFrames(amount, randomizer.RandomizeObjectPosition));
            else
                for (int i = 0; i < amount ; i++) SpawnRandom_Method(randomizer.RandomizeObjectPosition);
        }

        public void SpawnNoise(int amount, bool useMultiFrame)
        {
            if (useMultiFrame)
                StartCoroutine(SpawnOverFrames(amount, randomizer.PerlinRandomizeObjectPosition));
            else
                for (int i = 0; i < amount; i++) SpawnRandom_Method(randomizer.PerlinRandomizeObjectPosition);
        }

        public void DespawnAll()
        {
            for (int i = 0; i < objectPools.Length; i++)
            {
                objectPools[i].UnpoolAll();
            }
        }

        public void Spawn(int n, bool useMultiFrame)
        {
            if (usePerlinNoise) SpawnNoise(n, useMultiFrame);
            else SpawnRandom(n, useMultiFrame);
        }

        public void Spawn(int n) => Spawn(n, false);
    }
}
