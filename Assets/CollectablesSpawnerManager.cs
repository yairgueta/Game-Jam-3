using System;
using System.Collections;
using System.Collections.Generic;
using Spawners;
using UnityEngine;

[RequireComponent(typeof(SpawnersManager))]
public class CollectablesSpawnerManager : MonoBehaviour
{
    [SerializeField] private float baseRespawnTime = 10f;
    [SerializeField] private AnimationCurve spawningCurve;
    [SerializeField] private float softenFactor = 1f;
    private SpawnersManager spawnersManager;
    private float timer;

    private void Start()
    {
        spawnersManager = GetComponent<SpawnersManager>();
    }

    private void Update()
    {
        var f = spawningCurve.Evaluate(spawnersManager.CurrentSpawned / (float)spawnersManager.TotalPool);
        f = Mathf.Clamp01(f);
        timer -= Time.deltaTime * Mathf.Pow(f, softenFactor);
        if (timer > 0) return;
        timer = baseRespawnTime;
        spawnersManager.SpawnMany(1);
    }
}
