using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using Upgrader;

public class Wall : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth;
    private Collider2D wallCollider;
    private Upgradable upgradable;
    private float curHealth;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        curHealth = maxHealth;
        upgradable = GetComponent<Upgradable>();
        upgradable.onUpgrade += OnUpgrade;
    }

    public void TakeDamage(float damage)
    {
        curHealth -= damage;
        if (curHealth <= 0)
        {
            upgradable.ReduceToLevel(0);
            wallCollider.enabled = false;
        }
    }

    public void OnUpgrade()    //todo: event or upgradable holds??
    {
        curHealth = upgradable.GetCurGradeAttributes().healthPoints;
        wallCollider.enabled = true;
    }


}
