using System;
using System.Collections.Generic;
using System.Linq;
using Cycles;
using Events;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Spawners
{

    public class EnemiesSpawner : MonoBehaviour
    {
        [Tooltip("% of spawned enemies to spawn in a random spawner")][SerializeField][Range(0,1)] private float randomPercentage; 
        private List<Spawner> spawners;

        void Start()
        {
            spawners = FindObjectsOfType<Spawner>().ToList().FindAll(s => s.SpawnerType == Spawner.ObjectPoolType.Enemy);
            var listener = gameObject.AddComponent<GameEventListener>();
            listener.InitEvent(CyclesManager.Instance.NightSettings.OnCycleEnd);
            listener.response.AddListener(o=>DespawnEnemies());
        }

        public void SpawnEnemies(int n)
        {
            var randomCount = Mathf.FloorToInt(randomPercentage * n);
            var elseCount = n - randomCount;
            
            SpawnEvenly(elseCount);
            SpawnRandom(randomCount);
        }

        public void DespawnEnemies()
        {
            spawners.ForEach(s => s.DespawnAll());
        }
        
        private void SpawnEvenly(int n)
        {
            var alreadySpawned = 0;
            foreach (var spnr in spawners)
            {
                var c = n / spawners.Count;
                spnr.SpawnRandom(c);
                alreadySpawned += c;
            }
            if (n - alreadySpawned > 0) spawners[0].SpawnRandom(n - alreadySpawned);
        }

        private void SpawnRandom(int n)
        {
            for (int i = 0; i < n; i++)
            {
                var spnr = spawners[Random.Range(0, spawners.Count)];
                spnr.SpawnRandom();
            }
        }
    }
}
