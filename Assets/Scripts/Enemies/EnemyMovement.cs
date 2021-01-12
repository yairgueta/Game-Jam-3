using Enemies;
using UnityEngine;
using Pathfinding;
using UnityEngine.EventSystems;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private EnemySettings enemySettings;
    [SerializeField] private Transform enemyGFX;
    
    public Rigidbody2D rb;
    private int currentWaypoint;
    private Path currentPath;
    private Vector3 initialEnemyScale;
    private Seeker seeker;
    private bool isMoving;
    
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        initialEnemyScale = enemyGFX.localScale;

        StartFollow();
    }

    public void Move()
    {
        isMoving = true;
    }
    
    private void FixedUpdate()
    {
        if (currentPath != null && isMoving)
        {
            MoveToNextTarget();
            isMoving = false;
        }
    }

    public void StartFollow()
    {
        // enemySettings.enemyMode = Mode.Walking;
        // ChooseTarget();
        FindPath();
    }
    
    // private void ChooseTarget()
    // {
    //     var followIndicator = Random.Range(FollowPlayer, FollowWalls + 1);
    //     if (followIndicator == FollowPlayer)
    //     {
    //         enemySettings.target = playerTransform;
    //         return;
    //     }
    //     enemySettings.target = wallsPosition;
    // }
    
    private void OnPathComplete(Path path)
    {
        if (path.error) return;
        currentPath = path;
        currentWaypoint = 0; // to start at the beginning of the new path. 
    }
    
    private void FindPath()
    {
        InvokeRepeating(nameof(UpdatePath), 0f, enemySettings.pathRepeatRate);
        seeker.StartPath(rb.position, target.position, OnPathComplete);
    }
    
    private void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    
    private void MoveToNextTarget()
    {
        if (currentWaypoint >= currentPath.vectorPath.Count) return;
        Vector2 direction = ((Vector2) currentPath.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * enemySettings.speed;
        rb.velocity = force;
        float distance = Vector2.Distance(rb.position, currentPath.vectorPath[currentWaypoint]);

        if (distance < enemySettings.nextWaypointDistance)
        {
            currentWaypoint++;
        }
        UpdateDirection();
    }

    private void UpdateDirection()
    {
        var sign = Mathf.Sign( transform.position.x - target.position.x);
        enemyGFX.localScale = new Vector3(sign * initialEnemyScale.x, initialEnemyScale.y, initialEnemyScale.z);
    }
}
