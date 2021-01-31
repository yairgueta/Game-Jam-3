using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spawners.EnemiesSpawner
{
    public class EnemiesSpawner : MonoBehaviour
    {
        private Queue<Spawnable> enemiesQueue;
        public EnemiesSpawnerMinimapIcon minimapIcon { get; private set; }
        private void Start()
        {
            minimapIcon = GetComponentInChildren<EnemiesSpawnerMinimapIcon>();
            
            enemiesQueue = new Queue<Spawnable>();
            foreach (Transform child in transform)
            {
                var spnble = child.GetComponent<Spawnable>();
                if (spnble)
                {
                    spnble.Init(enemiesQueue.Enqueue);
                    spnble.gameObject.SetActive(false);
                }
            }
        }

        private void Spawn()
        {
            if (enemiesQueue.Count == 0)
            {
                Debug.LogError("Enemies pool is empty!");
                return;
            }

            var spnble = enemiesQueue.Dequeue();
            spnble.transform.position = transform.position;
            spnble.gameObject.SetActive(true);
        }

        IEnumerator SpawnManyCoroutine(int count, float totalTimeToSpawn)
        {
            WaitForSeconds delayBetweenEachSpawn = new WaitForSeconds(totalTimeToSpawn / (count - 1));
            for (var i = 0; i < count; i++)
            {
                Spawn();
                yield return delayBetweenEachSpawn;
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
