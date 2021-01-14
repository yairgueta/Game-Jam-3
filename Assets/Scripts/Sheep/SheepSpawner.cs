using System.Collections;
using System.Collections.Generic;
using Events;
using Player;
using UnityEngine;

public class SheepSpawner : MonoBehaviour
{

    private Spawner sheepSpawnerObject;
    [SerializeField] private MushroomCollectable mushroomCollectable;

    [SerializeField] private int sheepAmount;

    void Start()
    {
        sheepSpawnerObject = GetComponent<Spawner>();
        for (int i = 0; i < sheepAmount; i++)
        {
            sheepSpawnerObject.SpawnGameObject();
        }
    }


    public void SpawnNewSheep()
    {
        sheepSpawnerObject.SpawnGameObject();
    }

}
