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
    [SerializeField] private float timeBetweenSpawning = 5f;
    [SerializeField] private int roundsOfSpawning = 3;
    private int enemyNum;

    void Start()
    {
        spawnersManager = GetComponent<SpawnersManager>();
        roundsCounter = 8;
        var listener = gameObject.AddComponent<GameEventListener>();
        listener.InitEvent(CyclesManager.Instance.NightSettings.OnCycleStart);
        listener.response.AddListener(o => SpawnEnemies());
        
    }

    private void SpawnEnemies()
    {
        enemyNum = EnemiesAccordingToRounds();
        StartCoroutine(Spawn(roundsOfSpawning));
        roundsCounter++;
    }

    private IEnumerator Spawn(int i)
    {
        if (i > 0)
        {
            yield return new WaitForSeconds(timeBetweenSpawning);
            int num = Random.Range(1,8);
            spawnersManager.SpawnMany(num);
            i--;
            StartCoroutine(Spawn(i));
        }
    }

    private int EnemiesAccordingToRounds()
    {
        return roundsCounter;
    }

}
