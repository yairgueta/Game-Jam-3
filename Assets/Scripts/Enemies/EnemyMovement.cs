using UnityEngine;
using Pathfinding;

public enum Mode {Walking, AttackPlayer, AttackWall, AttackSheep}
public class EnemyMovement : MonoBehaviour
{
    private const int FollowPlayer = 0;
    private const int FollowWalls = 1;
    
    [SerializeField] private EnemySettings enemySettings;
    [SerializeField] private Transform enemyGFX;
    
    public Rigidbody2D rb;
    public Transform wallsPosition;
    public Transform playerTransform;
    
    private int currentWaypoint = 0;
    private Path currentPath;
    private Vector3 initialEnemyScale;
    private Seeker seeker;
    
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        initialEnemyScale = enemyGFX.localScale;

        StartFollow();
    }
    
    public void StartFollow()
    {
        enemySettings.enemyMode = Mode.Walking;
        ChooseTarget();
        FindPath();
    }
    
    private void ChooseTarget()
    {
        var followIndicator = UnityEngine.Random.Range(FollowPlayer, FollowWalls + 1);
        if (followIndicator == FollowPlayer)
        {
            enemySettings.target = playerTransform;
            return;
        }
        enemySettings.target = wallsPosition;
    }
    
    private void OnPathComplete(Path path)
    {
        if (path.error) return;
        currentPath = path;
        currentWaypoint = 0; // to start at the beginning of the new path. 
    }
    
    private void FindPath()
    {
        InvokeRepeating(nameof(UpdatePath), 0f, enemySettings.pathRepeatRate);
        seeker.StartPath(rb.position, enemySettings.target.position, OnPathComplete);
    }
    
    private void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, enemySettings.target.position, OnPathComplete);
        }
    }
    
    void FixedUpdate()
    {
        if (currentPath != null)
        {
            MoveToNextTarget();
        }
    }

    private void MoveToNextTarget()
        {
            if (currentWaypoint >= currentPath.vectorPath.Count) return;
            Vector2 direction = ((Vector2) currentPath.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * enemySettings.speed;
            var distanceToTarget = Vector2.Distance(rb.position, enemySettings.target.position);
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
        if (enemySettings.target.position.x > transform.position.x)
        {
            enemyGFX.localScale = new Vector3(-initialEnemyScale.x, initialEnemyScale.y, initialEnemyScale.z);
        }
        else if (enemySettings.target.position.x < transform.position.x)
        {
            enemyGFX.localScale = new Vector3(initialEnemyScale.x, initialEnemyScale.y, initialEnemyScale.z);
        }
    }
}
