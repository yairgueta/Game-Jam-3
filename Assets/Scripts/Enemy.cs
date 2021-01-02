using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] float speed = 0.01f;
    [SerializeField] private float nextWaypointDistance = 3f;
    [SerializeField] private Transform enemyGFX;

    private Path currentPath;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath;

    private Seeker seeker;
    private Rigidbody2D rigidbody2D;
    private Vector3 initialEnemyScale;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        initialEnemyScale = enemyGFX.localScale;

        InvokeRepeating("UpdatePath", 0f, .5f);
        seeker.StartPath(rigidbody2D.position, target.position, OnPathComplete);
    }

    private void UpdatePath()
    {
        if (seeker.IsDone() && ! reachedEndOfPath)
        {
            seeker.StartPath(rigidbody2D.position, target.position, OnPathComplete);
        }
    }

    private void OnPathComplete(Path path)
    {
        if (!path.error)
        {
            currentPath = path;
            currentWaypoint = 0; // to start at the beginning of the new path. 
        }
        else
        {
            Debug.LogWarning("Path finder generated an error.");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentPath == null) return;
        MoveToNextTarget();
    }

    private void MoveToNextTarget()
    {
        if (!(currentWaypoint >= currentPath.vectorPath.Count))
        {
            reachedEndOfPath = false;
            Vector2 direction = ((Vector2) currentPath.vectorPath[currentWaypoint] - rigidbody2D.position).normalized;
            Vector2 force = direction * speed;
            rigidbody2D.velocity = force;
            float distance = Vector2.Distance(rigidbody2D.position, currentPath.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }

            if (force.x >= Mathf.Epsilon)
            {
                enemyGFX.localScale = new Vector3(-initialEnemyScale.x, initialEnemyScale.y, initialEnemyScale.z);
            }
            else if (force.x <= Mathf.Epsilon)
            {
                enemyGFX.localScale = new Vector3(initialEnemyScale.x, initialEnemyScale.y, initialEnemyScale.z);
            }
        }
        else
        {
            reachedEndOfPath = true;
        }
    }
}
