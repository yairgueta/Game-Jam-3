using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnArray;
    private SpawnPlacing spawnPlacing;

    private List<Vector2> positionsToSpawn;
    private int spawnIndex = 0;

    // Start is called before the first frame update
    void Awake()
    {
        spawnPlacing = GetComponent<SpawnPlacing>();
        positionsToSpawn = new List<Vector2>();
        spawnPlacing.Initialize();
        Initialize();
        
    }
    
    private void Initialize()
    {
        foreach (var toSpawn in spawnArray)
        {
            positionsToSpawn.Add(spawnPlacing.GetVacantPosition());
            
        }
        spawnIndex = 0;
    }

    public void SpawnGameObject()
    {
        if (spawnIndex == spawnArray.Length)
        {
            Initialize();
        }
        spawnArray[spawnIndex].transform.position = positionsToSpawn[spawnIndex];
        spawnArray[spawnIndex].SetActive(true);
        if (spawnPlacing.spawnSettings.updateSpotsAfterSpawn)
        {
            spawnPlacing.RemovePosition(spawnArray[spawnIndex].transform.position);
        }
        spawnIndex++;
    }

    public void SpawnNumberOfGameObjects(int num)
    {
        if (num <= 0) return;
        for (int i = 0; i < num; i++)
        {
            SpawnGameObject();
        }
    }
}
