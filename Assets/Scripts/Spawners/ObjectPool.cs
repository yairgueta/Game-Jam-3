using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spawners
{
    public class ObjectPool : MonoBehaviour
    {
        public GameObject PooledPrefab => pooledPrefab;
        
        [SerializeField] private GameObject pooledPrefab;
        [SerializeField] private int totalPoolAmount;

        private Queue<int> queuePool;
        private Spawnable[] pooledObjects;
        
        public int CurrentPooled => totalPoolAmount - queuePool?.Count ?? 0;
        public int TotalPool => totalPoolAmount;
        public bool IsFull => CurrentPooled == totalPoolAmount;

        private void Awake()
        {
            ObjectPoolManager.Instance.pools.Add(this);
            DontDestroyOnLoad(gameObject);
            
            queuePool = new Queue<int>();
            pooledObjects = new Spawnable[totalPoolAmount];
            InstantiateRemainingPoolObjects();
        }

        public void InitializePool(GameObject pooledPrefab, int totalPoolAmount)
        {
            this.pooledPrefab = pooledPrefab;
            this.totalPoolAmount = totalPoolAmount;

            queuePool = new Queue<int>();
            pooledObjects = new Spawnable[totalPoolAmount];
            InstantiateRemainingPoolObjects();
        }
        
        private void InstantiateRemainingPoolObjects()
        {
            int i = 0;
            foreach (Transform child in transform)
            {
                var p = child.GetComponent<Spawnable>();
                if (p == null) continue;
                p.onThisDeath += SpawnableDeath;
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
                var p = Instantiate(pooledPrefab, transform).GetComponent<Spawnable>();
                p.onThisDeath += SpawnableDeath;
                p.spawnerIndex = i;
                pooledObjects[i] = p;
                p.gameObject.SetActive(false);
            }
        }
        
        private void SpawnableDeath(Spawnable spawnable)
        {
            queuePool.Enqueue(spawnable.spawnerIndex);
        }

        public Spawnable Pool()
        {
            if (queuePool.Count == 0)
            {
                Debug.LogWarning(name + ": Pool is Out of objects, None was spawned");
                return null;
            }

            return pooledObjects[queuePool.Dequeue()];
        }

        public void UnpoolAll()
        {
            for (int i = 0; i < pooledObjects.Length ; i++)
            {
                var p = pooledObjects[i];
                if (p.gameObject.activeInHierarchy)
                    p.gameObject.SetActive(false);
            }
        }
    }
}