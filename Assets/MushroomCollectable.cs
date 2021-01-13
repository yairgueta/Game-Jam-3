using System.Collections;
using System.Collections.Generic;
using Collectables;
using UnityEngine;

[CreateAssetMenu(menuName = "Collectables/Mushroom_@@")]

public class MushroomCollectable : CollectableObject
{
    [SerializeField] private GameObject sheepPrefab;
    [SerializeField] private Vector3 pos;
    public override void OnCollected()
    {
        // find place in sheep spawner and instantiate there
        
        // Instantiate(sheepPrefab, pos, Quaternion.identity);
    }
}
