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
        [SerializeField] private AnimationCurve spawningSpeedCurve;
        [SerializeField] private float respawnSpeed = 1f;
        private SpawnersManager spawnersManager;
        private float timer;

        private void Start()
        {
            spawnersManager = GetComponent<SpawnersManager>();
        }

        private void Update()
        {
            var f = spawningSpeedCurve.Evaluate(spawnersManager.CurrentSpawned / (float) spawnersManager.TotalPool);
            f = Mathf.Clamp(f, 0, 1);
            timer -= Time.deltaTime * respawnSpeed * f;
            if (timer > 0) return;
            timer = baseRespawnTime;
            spawnersManager.SpawnMany(1);
        }
    }
}
