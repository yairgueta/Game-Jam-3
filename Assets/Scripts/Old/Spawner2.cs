using System.Collections.Generic;
using UnityEngine;

namespace Spawners
{
    public class Spawner2 : MonoBehaviour
    {
        [SerializeField] private GameObject spawnablePrefab;
        [SerializeField] private int spawnAmount;
        private Queue<Spawnable> spawnQueue = new Queue<Spawnable>();
        private SpawnPlacing spawnPlacing;

        // Start is called before the first frame update
        void Awake()
        {
            spawnPlacing = GetComponent<SpawnPlacing>();
            spawnPlacing.Initialize();
            for (var i = 0; i < spawnAmount; i++)
            {
                var toSpawn = Instantiate(spawnablePrefab).GetComponent<Spawnable>();
                if (toSpawn == null)
                {
                    Debug.LogWarning(gameObject.name+" cannot spawn");
                    return;
                }
                // toSpawn.Spawn(this);
                toSpawn.gameObject.SetActive(false);
            }
        }

        public void SpawnGameObject()
        {
            var toSpawn = spawnQueue.Dequeue();
            toSpawn.transform.position = spawnPlacing.GetVacantPosition();
            toSpawn.gameObject.SetActive(true);
            if (spawnPlacing.spawnSettings.updateSpotsAfterSpawn)
            {
                spawnPlacing.RemovePosition(toSpawn.transform.position);
            }
        }

        public void SpawnNumberOfGameObjects(int num)
        {
            if (num <= 0) return;
            for (int i = 0; i < num; i++)
            {
                SpawnGameObject();
            }
        }

        public void SpawnableDeath(Spawnable spawnable)
        {
            if (!spawnPlacing.spawnSettings.updateSpotsAfterSpawn) return;
            spawnPlacing.AddPosition(spawnable.transform.position);
            spawnQueue.Enqueue(spawnable);
        }

    
    }
}
