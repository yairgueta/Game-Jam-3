using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawnable : MonoBehaviour
{

    private Spawner spawner;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    internal void Spawn(Spawner spawner)
    {
        this.spawner = spawner;
    }

    protected virtual void OnDisable()
    {
        spawner.SpawnableDeath(this);
    }
}
