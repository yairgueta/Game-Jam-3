using System;
using System.Collections;
using System.Collections.Generic;
using Cycles;
using UnityEngine;

namespace Spawners.EnemiesSpawner
{
    public class EnemiesSpawnerManager : Singleton<EnemiesSpawnerManager>
    {
        public List<EnemiesSpawner> enemiesSpawners { get; private set; }
        [SerializeField] private EnemiesRounds enemiesRounds;
        
        private int currentRoundIndex;
        private List<EnemiesSpawner> currentActiveSpawners;
        
        public Round CurrentRound => enemiesRounds[currentRoundIndex];
        private void Start()
        {
            currentActiveSpawners = new List<EnemiesSpawner>();
            enemiesSpawners = new List<EnemiesSpawner>();
            foreach (Transform child in transform)
            {
                var spnr = child.GetComponent<EnemiesSpawner>();
                if(spnr) enemiesSpawners.Add(spnr);
            }
            CyclesManager.Instance.NightSettings.OnCycleStart.Register(gameObject, o => SpawnCurrentRound());
        }

        private void SpawnCurrentRound()
        {
            print(currentRoundIndex);
            currentActiveSpawners.Clear();

            for (int i = 0; i < enemiesSpawners.Count; i++)
            {
                if (((1 << i) & CurrentRound.activeSpawners) != 0)
                    currentActiveSpawners.Add(enemiesSpawners[i]);
            }
            
            var count = CurrentRound.enemiesCount / currentActiveSpawners.Count;

            switch (CurrentRound.spawnType)
            {
                case SpawnType.OneAfterOne:
                    var time = CurrentRound.timeToSpawn / currentActiveSpawners.Count;
                    StartCoroutine(SpawnOneAfterOne(count, time));
                    break;
                case SpawnType.AllTogether:
                    
                    currentActiveSpawners.ForEach( spnr => spnr.SpawnMany(count, CurrentRound.timeToSpawn));
                    break;
            }

            currentRoundIndex = Math.Min(enemiesRounds.rounds.Count, currentRoundIndex + 1);
        }

        IEnumerator SpawnOneAfterOne(int count, float time)
        {
            var delay = new WaitForSeconds(time);

            for (int i = 0; i < currentActiveSpawners.Count; i++)
            {
                currentActiveSpawners[i].SpawnMany(count, time);
                yield return delay;
            }        
        
        }
    }
}