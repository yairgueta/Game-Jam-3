using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/ Spawn Settings")]
public class SpawnSettings : ScriptableObject
{
    [Header("Game Boundaries")] 
    public float minGameX;
    public float minGameY;
    public float maxGameX;
    public float maxGameY;

    [Header("Dead zone boundaries")] 
    public float deadZoneMinX;
    public float deadZoneMinY;
    public float deadZoneMaxX;
    public float deadZoneMaxY;

    [Header("Attributes")] 
    public float spawnUnitSize;
    public bool useDeadZone;
    public bool updateSpotsAfterSpawn;
}
