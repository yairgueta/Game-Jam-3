using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] float speed = 200f;
    [SerializeField] private float nextWaypointDistance = 3f;

    private Path currentPath;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath;

    private Seeker seeker;
    private Rigidbody2D rigidbody2D;
    
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        seeker.StartPath(rigidbody2D.position, target.position, OnPathComplete);
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
    void Update()
    {
        
    }
}
