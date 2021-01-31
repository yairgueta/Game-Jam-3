using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Pathfinding;
using Pathfinding.Util;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Spawners
{
    [RequireComponent(typeof(Collider2D), typeof(SpawnerRandomizer))]
    public class Spawner : MonoBehaviour
    {
        [HideInInspector][SerializeField] private GameObject[] pooledPrefab;
        [SerializeField] private int totalPoolAmount;
        [HideInInspector][SerializeField] private bool useOverFrameSpawning = true;
        [HideInInspector][SerializeField] private float overFrameTimeout = .5f;
        [HideInInspector][SerializeField] private int startingAmount;
        [HideInInspector][SerializeField] private bool usePerlinNoise;
        [HideInInspector][SerializeField] private float[] percentageToSpawn;
        
        private SpawnerRandomizer randomizer;
        private Queue<int> queuePool;
        private Spawnable[] pooledObjects;
        private Action gameManagerWaitingListOnFinishTask;
        public int CurrentPooled => totalPoolAmount - queuePool?.Count ?? 0;
        public int TotalPool => totalPoolAmount;
        public bool IsFull => CurrentPooled == totalPoolAmount;

        private void Awake()
        {
            randomizer = GetComponent<SpawnerRandomizer>();
            queuePool = new Queue<int>();
            pooledObjects = new Spawnable[totalPoolAmount];
            gameObject.layer = LayerMask.NameToLayer("Spawner");
        }

        public GameObject GetRandomPrefab()
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
            InstatiateRemainingPoolObjects();
            Spawn(startingAmount, useOverFrameSpawning);
            if (useOverFrameSpawning)
                gameManagerWaitingListOnFinishTask = GameManager.Instance.RegisterToWaitingList();
        }

        private void InstatiateRemainingPoolObjects()
        {
            int i = 0;
            foreach (Transform child in transform)
            {
                var p = child.GetComponent<Spawnable>();
                if (p == null) continue;
                p.spawnerIndex = i;
                pooledObjects[i] = p;
                if (child.gameObject.activeSelf)
                    child.gameObject.SetActive(false);
                else
                    queuePool.Enqueue(i);
                i++;
            }

            for (; i < totalPoolAmount; i++)
            {
                var p = Instantiate(GetRandomPrefab(), transform).GetComponent<Spawnable>();
                p.Init(SpawnableDeath);
                p.spawnerIndex = i;
                pooledObjects[i] = p;
                p.gameObject.SetActive(false);
            }
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
            // bs = p.physicsCollider;
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
            foreach (var p in pooledObjects)
                p.gameObject.SetActive(false);
        }

        public void Spawn(int n, bool useMultiFrame)
        {
            if (usePerlinNoise) SpawnNoise(n, useMultiFrame);
            else SpawnRandom(n, useMultiFrame);
        }

        public void Spawn(int n) => Spawn(n, false);
        
        public void SpawnAll()
        {
            Spawn(totalPoolAmount - CurrentPooled);
        }
    }
}
