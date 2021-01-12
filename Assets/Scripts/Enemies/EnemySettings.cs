using Events;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Enemy Settings")]
public class EnemySettings : ScriptableObject
{
    [Header("Attributes")]
    public float attackPower = 1;
    public float attackDistance = 3f;
    public float health = 5f;
    public float speed = 15f;
    public float attackDelay = 3f;

    [Header("AI Attributes")]
    public float nextWaypointDistance = 3f;
    public float pathRepeatRate = .5f;
    public float changeTargetRate = 5f;

    [Header("Events")] 
    public GameEvent onDeath;

    public void UpdateLife(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        onDeath.Raise();
        health = 5f; //TODO: delete
    }
}