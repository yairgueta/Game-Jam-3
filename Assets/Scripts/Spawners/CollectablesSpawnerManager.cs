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
        [SerializeField] private RespawnMethod respawnMethod;
        private SpawnersManager spawnersManager;
        private float timer;

        private void Start()
        {
            SheepSettings sheepSettings = AssetBundle.FindObjectOfType<SheepSettings>();
            spawnersManager = GetComponent<SpawnersManager>();
            toMethods = new Dictionary<RespawnMethod, Func<float>>
            {
                {
                    RespawnMethod.IncreaseAccordingToSheep,
                    () => sheepSettings.sheeps.Count / (float) sheepSettings.maxSheepInScene
                },
                {
                    RespawnMethod.IncreaseAccordingToPoolPercentage,
                    () => spawnersManager.CurrentSpawned / (float) spawnersManager.TotalPool
                }
            };
        }

        private void Update()
        {
            var f = spawningCurve.Evaluate(toMethods[respawnMethod]());
            f = Mathf.Clamp01(f);
            timer -= Time.deltaTime * Mathf.Pow(f, softenFactor);
            if (timer > 0) return;
            timer = baseRespawnTime;
            spawnersManager.SpawnMany(1);
        }
        
        

        private enum RespawnMethod
        {
            IncreaseAccordingToPoolPercentage,
            IncreaseAccordingToSheep,
        }

        private Dictionary<RespawnMethod, Func<float>> toMethods;
    }
}
