using System;
using System.Collections;
using System.Collections.Generic;
using Cycles;
using UnityEngine;

namespace Spawners.EnemiesSpawner
{
    public class EnemiesSpawnerManager : MonoBehaviour
    {
        public static EnemiesSpawnerManager Instance { get; private set; }
        
        public List<EnemiesSpawner> enemiesSpawners { get; private set; }
        [SerializeField] private EnemiesRounds enemiesRounds;
        
        private int currentRoundIndex = -1;
        private List<EnemiesSpawner> currentActiveSpawners;
        
        public Round CurrentRound => enemiesRounds[currentRoundIndex];

        private void OnEnable()
        {
            Instance = this;
        }

        private void OnDisable()
        {
            Instance = null;
        }

        private void Start()
        {
            currentActiveSpawners = new List<EnemiesSpawner>();
            enemiesSpawners = new List<EnemiesSpawner>();
            foreach (Transform child in transform)
            {
                var spnr = child.GetComponent<EnemiesSpawner>();
                if(spnr) enemiesSpawners.Add(spnr);
            }
            CyclesManager.Instance.DaySettings.OnCycleStart.Register(gameObject, o => GetNextRoundActiveSpawners());
            CyclesManager.Instance.NightSettings.OnCycleStart.Register(gameObject, o => SpawnCurrentRound());
        }

        private void SpawnCurrentRound()
        {
            var count = CurrentRound.enemiesCount / currentActiveSpawners.Count;

            switch (CurrentRound.spawnType)
            {
                case SpawnType.OneAfterOne:
                    var time = CurrentRound.timeToSpawn / currentActiveSpawners.Count;
                    StartCoroutine(SpawnOneAfterOne(count, time));
                    break;
                case SpawnType.AllTogether:
                    
                    currentActiveSpawners.ForEach(spnr => spnr.SpawnMany(count, CurrentRound.timeToSpawn));
                    break;
            }
        }

        private void GetNextRoundActiveSpawners()
        {
            currentActiveSpawners.Clear();
            currentRoundIndex = Math.Min(enemiesRounds.rounds.Count, currentRoundIndex + 1);

            for (int i = 0; i < enemiesSpawners.Count; i++)
            {
                if (((1 << i) & CurrentRound.activeSpawners) != 0)
                {
                    var enemiesSpawner = enemiesSpawners[i];
                    enemiesSpawner.minimapIcon.AnimateEnemiesWarning();
                    currentActiveSpawners.Add(enemiesSpawner);
                }
            }
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