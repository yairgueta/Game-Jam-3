using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Spawner : MonoBehaviour
{
    [FormerlySerializedAs("spawnArray")] [SerializeField] private Queue<Spawnable> spawnQueue;
    private SpawnPlacing spawnPlacing;

    // Start is called before the first frame update
    void Start()
    {
        spawnPlacing = GetComponent<SpawnPlacing>();
        spawnPlacing.Initialize();
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
