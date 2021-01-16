using System;
using System.Collections.Generic;
using System.Linq;
using Cycles;
using Events;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Spawners
{

    public class SpawnersManager : MonoBehaviour
    {
        [Tooltip("% of spawned Objects to spawn in a random spawner")][SerializeField][Range(0,1)] private float randomPercentage;
        private List<Spawner> spawners;

        public int CurrentSpawned => spawners.Sum(s => s.CurrentPooled);
        public int TotalPool => spawners.Sum(s => s.TotalPool);

        private void Start()
        {
            spawners = transform.GetComponentsInChildren<Spawner>().ToList();
        }
        
        public void RespawnAll()
        {
            spawners.ForEach(s =>
            {
                s.DespawnAll();
                s.SpawnAll();
            });
        }
        
        public void SpawnMany(int n)
        {
            var randomCount = Mathf.FloorToInt(randomPercentage * n);
            var elseCount = n - randomCount;
            
            SpawnEvenly(elseCount);
            SpawnRandom(randomCount);
        }

        public void DespawnAll()
        {
            spawners.ForEach(s => s.DespawnAll());
        }
        
        private void SpawnEvenly(int n)
        {
            var alreadySpawned = 0;
            foreach (var spnr in spawners)
            {
                var c = n / spawners.Count;
                spnr.Spawn(c);
                alreadySpawned += c;
            }
            if (n - alreadySpawned > 0) spawners[0].Spawn(n - alreadySpawned);
        }

        private void SpawnRandom(int n)
        {
            for (int i = 0; i < n; i++)
            {
                var spnr = spawners[Random.Range(0, spawners.Count)];
                while (spnr.IsFull) spnr = spawners[Random.Range(0, spawners.Count)];
                spnr.Spawn(1);
            }
        }
    }
}
