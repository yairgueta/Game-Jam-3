using System.Collections;
using System.Collections.Generic;
using Cycles;
using Events;
using Spawners;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    private int roundsCounter;
    private SpawnersManager spawnersManager;

    void Start()
    {
        spawnersManager = GetComponent<SpawnersManager>();
        roundsCounter = 8;
        var listener = gameObject.AddComponent<GameEventListener>();
        listener.InitEvent(CyclesManager.Instance.NightSettings.OnCycleStart);
        listener.response.AddListener(o => Spawn());
    }

    public void Spawn()
    {
        spawnersManager.SpawnMany(EnemiesAccordingToRounds()); 
        roundsCounter++;
    }

    private int EnemiesAccordingToRounds()
    {
        return roundsCounter;
    }

}
