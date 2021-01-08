using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Enemy Settings")]
public class EnemySettings : ScriptableObject
{
    [Header("Attributes")]
    public int attackPower = 1;
        
    public float attackDistance = 3f;
    public float changeTargetRate = 5f;
    public float health = 5f;
    public float pathRepeatRate = .5f;
    public float speed = 15f;
    public float nextWaypointDistance = 3f;
    
    
    
    public Transform target;
    public Mode enemyMode;


    // Start is called before the first frame update
    void Start()
    {
        
    }
    
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
        Debug.Log("enemy died");
        health = 5f; //TODO: delete
    }
}
