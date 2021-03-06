using System;
using Events;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Spawners
{
    [RequireComponent(typeof(SpawnersManager))]
    public class MushroomSpawnerManager : MonoBehaviour
    {
        [SerializeField] private AnimationCurve curve;
        [Tooltip("make it >1 to have more randomness. The bigger this number, the more random.")][SerializeField] private float randomnessFacter;
        [SerializeField] private SheepSettings sheepSettings;
        private SpawnersManager spawnersManager;
        private void Start()
        {
            spawnersManager = GetComponent<SpawnersManager>();

            Cycles.CyclesManager.Instance.EclipseSettings.OnCycleStart.Register(gameObject, OnEclipseStart_Listener);
            Cycles.CyclesManager.Instance.EclipseSettings.OnCycleEnd.Register(gameObject, OnEclipseEnd_Listener);
            
            if (sheepSettings != null) return;
            var sheepSetts = AssetBundle.FindObjectsOfType<SheepSettings>();
            if (sheepSetts.Length > 1) 
                Debug.LogWarning("Mushroom Spawner ambiguity: None sheep settings was provided and there are more then one in project!");
            if (sheepSetts.Length == 0)
            {
                Debug.LogError("Mushroom Spawner Problem: None Sheep settings were found!");
                return;
            }
            sheepSettings = sheepSetts[0];
        }

        private void OnEclipseEnd_Listener(object o)
        {
            //TODO: Maybe some fadeaway effect
            spawnersManager.DespawnAll();
        }
        
        private void OnEclipseStart_Listener(object o)
        {
            // TODO: some Fadein effect??
            spawnersManager.SpawnMany(RandomAmountToSpawn);
        }
        

        // t = current count / max count (of sheep)
        float DecreaseFunction(float t)
        {
            return Mathf.Clamp01(curve.Evaluate(t));
        }

        private int AmountToSpawn => Mathf.FloorToInt(sheepSettings.maxSheepInScene *
                                                      DecreaseFunction(sheepSettings.sheeps.Count / (float) sheepSettings.maxSheepInScene));

        private int RandomAmountToSpawn => Random.Range(Mathf.FloorToInt(AmountToSpawn / randomnessFacter), AmountToSpawn);
        
        
        
    }
}