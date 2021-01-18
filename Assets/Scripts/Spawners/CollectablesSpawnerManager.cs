using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Spawners
{
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
            SheepSettings sheepSettings = AssetBundle.FindObjectOfType<SheepSettings>();
            spawnersManager = GetComponent<SpawnersManager>();
        }

        private void Update()
        {
            var f = spawningCurve.Evaluate(spawnersManager.CurrentSpawned / (float) spawnersManager.TotalPool);
            f = Mathf.Clamp(f, 0, 2);
            timer -= Time.deltaTime * Mathf.Pow(f, softenFactor);
            if (timer > 0) return;
            timer = baseRespawnTime;
            spawnersManager.SpawnMany(1);
        }
    }
}
