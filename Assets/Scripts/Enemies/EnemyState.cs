using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Pathfinding;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform wallsPosition;
    [SerializeField] private EnemySettings enemySettings;
    
    private AIDestinationSetter destinationSetter;
    private bool updateTarget = true;

    // Start is called before the first frame update
    void Start()
    {
        destinationSetter = GetComponent<AIDestinationSetter>();
    }

    private void Update()
    {
        if (updateTarget)
        {
            updateTarget = false;
            ManageTarget();
        }
    }

    private IEnumerator DelayChange()
    {
        yield return new WaitForSeconds(enemySettings.targetRefreshTime);
        updateTarget = true;
    }

    private void ManageTarget()
    {
        var enemyPos = transform.position;
        var distanceFromPlayer = Vector2.Distance(playerTransform.position, enemyPos);
        var distanceFromWalls = Vector2.Distance(wallsPosition.position, enemyPos);
        destinationSetter.target = distanceFromPlayer < distanceFromWalls ? playerTransform : wallsPosition;
        StartCoroutine(DelayChange());
    }
}
