using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spawners.EnemiesSpawner
{
    [CreateAssetMenu(menuName = "Enemies Round")]
    public class EnemiesRounds : ScriptableObject
    {
        public List<Round> rounds;

        public Round this[int i] => rounds[i];
    } 
    [Serializable]
    public class Round
    {
        public int activeSpawners;
        public int enemiesCount;
        public float timeToSpawn;
        public SpawnType spawnType;
    }

    public enum SpawnType
    {
        OneAfterOne,
        AllTogether
    }
}