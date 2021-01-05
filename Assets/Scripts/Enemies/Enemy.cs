using System;
using System.Collections;
using UnityEngine;
using Pathfinding;

// an enemy in the game.
public class Enemy : MonoBehaviour, IDamageable
{
    private enum EnemyMode {Walking, AttackPlayer, AttackWall, AttackSheep}
    
    private const int FollowPlayer = 0;
    private const int FollowWalls = 1;
    
    [SerializeField] float speed = 0.01f;
    [SerializeField] private float nextWaypointDistance = 3f;
    [SerializeField] private int attackPower;
    [SerializeField] private Transform enemyGFX;
    [SerializeField] private Transform wallsPosition;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float attackDistance = 3f;
    [SerializeField] private float changeTargetRate = 5f;
    [SerializeField] private float health = 5f;

    private Path currentPath;
    private int currentWaypoint = 0;
    private Transform target;
    private EnemyMode enemyMode;
    private IDamageable attackedObject;
    private bool shouldChangeTarget;

    private Seeker seeker;
    private Rigidbody2D rb;
    private Vector3 initialEnemyScale;
    private float pathRepeatRate = .5f;
    private bool canAttack = true;


    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        initialEnemyScale = enemyGFX.localScale;

        StartFollow();
        InvokeRepeating(nameof(UpdateTarget), changeTargetRate, changeTargetRate);
    }
    
    void FixedUpdate()
    {
        if (currentPath == null) return;
        MoveToNextTarget();
        if (enemyMode != EnemyMode.Walking)
        {
            Attack(attackedObject);
        }
    }

    // starts to follow current target.
    private void StartFollow()
    {
        enemyMode = EnemyMode.Walking;
        ChooseTarget();
        FindPath();
    }
    
    // chooses a new target to follow.
    private void ChooseTarget()
    {
        var followIndicator = UnityEngine.Random.Range(FollowPlayer, FollowWalls + 1);
        if (followIndicator == FollowPlayer)
        {
            target = playerTransform;
            return;
        }
        target = wallsPosition;
    }

    // finds a path to the current target.
    private void FindPath()
    {
        InvokeRepeating(nameof(UpdatePath), 0f, pathRepeatRate);
        seeker.StartPath(rb.position, target.position, OnPathComplete);
    }
    
    // updates the path to the current target.
    private void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    // called when the last path has been completed.
    private void OnPathComplete(Path path)
    {
        if (path.error) return;
        currentPath = path;
        currentWaypoint = 0; // to start at the beginning of the new path. 
    }

    // moves the enemy towards the new target.
    private void MoveToNextTarget()
    {
        if (currentWaypoint >= currentPath.vectorPath.Count) return;
        Vector2 direction = ((Vector2) currentPath.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed;
        var distanceToTarget = Vector2.Distance(rb.position, playerTransform.position);
        if (distanceToTarget >= 2f)
        {
            rb.velocity = force;
            float distance = Vector2.Distance(rb.position, currentPath.vectorPath[currentWaypoint]);

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
        
    }

    // updates the target every "changeTargetRate" time according to the nearest target to the enemy.
    private void UpdateTarget()
    {
        // if (enemyMode != EnemyMode.Walking) return;
        var wallsDistance = Vector2.Distance(wallsPosition.position, rb.position);
        var playerDistance = Vector2.Distance(playerTransform.position, rb.position);
        if (target.position == playerTransform.position && wallsDistance < playerDistance)
        {
            target = wallsPosition;
            if (enemyMode != EnemyMode.AttackPlayer)
            {
                enemyMode = EnemyMode.Walking;
            }
        }
        else if(playerDistance < wallsDistance)
        {
            target = playerTransform;
            if (enemyMode != EnemyMode.AttackWall)
            {
                enemyMode = EnemyMode.Walking;
            }
        }
    }

    // attacks a damageable target.
    private void Attack(IDamageable damageable)
    {
        if (!shouldChangeTarget && Vector2.Distance(rb.position, target.position) <= attackDistance && canAttack)
        {
            damageable.TakeDamage(attackPower);
            canAttack = false;
            StartCoroutine(DelayAttack());
        }
        else if (shouldChangeTarget)
        {
            StartFollow();
        }
    }

    // delays the attack rate.
    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(3f);
        canAttack = true;
    }

    // sets the target to a target that enemy collided with.
    private void SetCollisionTarget(Transform newTarget, EnemyMode mode, IDamageable toAttack)
    {
        enemyMode = mode;
        target = newTarget;
        attackedObject = toAttack;
        shouldChangeTarget = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("enemy died");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            SetCollisionTarget(other.gameObject.transform, EnemyMode.AttackWall,
                other.gameObject.GetComponent<IDamageable>());
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            SetCollisionTarget(other.gameObject.transform, EnemyMode.AttackPlayer, 
                other.gameObject.GetComponent<IDamageable>());
        }
        else if (other.gameObject.CompareTag("Sheep"))
        {
            SetCollisionTarget(other.gameObject.transform, EnemyMode.AttackSheep,
                other.gameObject.GetComponent<IDamageable>());
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall") && enemyMode == EnemyMode.AttackWall)
        {
            shouldChangeTarget = true;
        }
        else if (other.gameObject.CompareTag("Player") && enemyMode == EnemyMode.AttackPlayer)
        {
            shouldChangeTarget = true;
        }
        else if (other.gameObject.CompareTag("Sheep") && enemyMode == EnemyMode.AttackSheep)
        {
            shouldChangeTarget = true;
        }
    }
}
