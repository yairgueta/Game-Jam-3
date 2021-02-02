using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Spawners.EnemiesSpawner
{
    public class EnemiesSpawner : MonoBehaviour
    {

        [SerializeField] private GameObject enemyPrefab;
        private ObjectPool enemiesPool;

        private void OnDisable()
        {
            enemiesPool.UnpoolAll();
        }

        public EnemiesSpawnerMinimapIcon minimapIcon { get; private set; }
        private void Start()
        {
            minimapIcon = GetComponentInChildren<EnemiesSpawnerMinimapIcon>();

            enemiesPool = ObjectPoolManager.Instance.GetObjectPool(enemyPrefab);
        }

        private void Spawn()
        {
            var spnble = enemiesPool.Pool();
            spnble.transform.position = transform.position;
            spnble.gameObject.SetActive(true);
        }

        IEnumerator SpawnManyCoroutine(int count, float totalTimeToSpawn)
        {
            WaitForSeconds delayBetweenEachSpawn = new WaitForSeconds(totalTimeToSpawn / (count - 1));
            for (var i = 0; i < count; i++)
            {
                yield return delayBetweenEachSpawn;
                Spawn();
                Debug.Log("1");
            }
        }
        public void SpawnMany(int count, float totalTimeToSpawn)
        {
            if (count == 1)
            {
                Spawn();
                return;
            }

            StartCoroutine(SpawnManyCoroutine(count, totalTimeToSpawn));
        }
    }
}
