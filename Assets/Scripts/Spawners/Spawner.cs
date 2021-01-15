using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spawners
{
    [RequireComponent(typeof(Collider2D), typeof(SpawnerRandomizer))]
    public class Spawner : MonoBehaviour
    {

        [SerializeField] private GameObject pooledPrefab;
        [SerializeField] private int poolAmount;
        [SerializeField] private int startingAmount;

        private SpawnerRandomizer randomizer;
        private Queue<Spawnable> queuePool;

        private void Awake()
        {
            queuePool = new Queue<Spawnable>();
        }

        private void Start()
        {
            randomizer = GetComponent<SpawnerRandomizer>();
            for (int i = 0; i < poolAmount; i++)
            {
                var p = Instantiate(pooledPrefab, transform).GetComponent<Spawnable>();
                p.Init(this);
                p.gameObject.SetActive(false);
            }

            for (int i = 0; i < startingAmount; i++) SpawnRandom();
        }

        internal void SpawnableDeath(Spawnable spawnable)
        {
            queuePool.Enqueue(spawnable);
        }

        public void SpawnRandom()
        {
            if (queuePool.Count == 0)
            {
                Debug.LogWarning(name + ": Pool spawner is Out of objects, None was spawned");
                return;
            }
            var p = queuePool.Dequeue();
            p.gameObject.SetActive(true);
            randomizer.RandomizeObjectPosition(p.gameObject);
        }
    }
}
