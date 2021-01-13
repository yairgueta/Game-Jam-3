using System;
using System.Collections;
using System.Collections.Generic;
using Collectables;
using UnityEngine;

[CreateAssetMenu(menuName = "Collectables/Mushroom_@@")]

public class MushroomCollectable : CollectableObject
{
    [SerializeField] private Spawner spawner;

    private void OnEnable()
    {
        spawner = GameObject.Find("SheepSpawner").GetComponent<Spawner>();
    }

    public override void OnCollected()
    {
        spawner.SpawnGameObject();
    }
}
