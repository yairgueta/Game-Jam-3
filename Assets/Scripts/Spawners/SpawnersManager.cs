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
        public int nextSpawnsNumber;
        [SerializeField] private RespawnMethod respawnMethod;
        [Tooltip("% of spawned Objects to spawn in a random spawner")][SerializeField][Range(0,1)] private float randomPercentage;
        [SerializeField] private Spawner.ObjectPoolType spawnersType;
        [SerializeField] private CycleObject cycleLife;
        
        private List<Spawner> spawners;

        private void Start()
        {
            if (spawnersType == Spawner.ObjectPoolType.Other)
            {
                Debug.LogWarning("You Should NOT have Spawner manager for 'other' spawner type!!!");
            }
            spawners = FindObjectsOfType<Spawner>().ToList().FindAll(s => s.SpawnerType == spawnersType);
            var listener = gameObject.AddComponent<GameEventListener>();
            listener.InitEvent(cycleLife.OnCycleEnd);
            listener.response.AddListener(o=>DespawnAll());
            
            listener = gameObject.AddComponent<GameEventListener>();
            listener.InitEvent(cycleLife.OnCycleStart);
            listener.response.AddListener(o=>Respawn());
        }

        private void Respawn()
        {
            switch (respawnMethod)
            {
                case RespawnMethod.RandomBetweenSpawners:
                    SpawnMany(nextSpawnsNumber);
                    break;
                case RespawnMethod.AllInAllSpawners:
                    spawners.ForEach(s =>
                    {
                        s.DespawnAll();
                        s.SpawnAll();
                    });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
                spnr.Spawn(1);
            }
        }

        public enum RespawnMethod
        {
            RandomBetweenSpawners,
            AllInAllSpawners
        }
    }
}
