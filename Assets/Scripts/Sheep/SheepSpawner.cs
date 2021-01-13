using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;

public class SheepSpawner : MonoBehaviour
{

    [SerializeField] private Spawner sheepSpawnerObject;
    [SerializeField] private MushroomCollectable mushroomCollectable;
    

    void Start()
    {
        sheepSpawnerObject = GetComponent<Spawner>();
    }


    public void SpawnNewSheep()
    {
        sheepSpawnerObject.SpawnGameObject();
    }

}
