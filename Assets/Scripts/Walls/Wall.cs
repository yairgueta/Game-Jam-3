using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using Upgrader;

public class Wall : MonoBehaviour, IEnemyDamage
{
    [SerializeField] private Collider2D wallCollider;
    private Upgradable upgradable;
    private float curHealth;
    [SerializeField] private float crackedPercentage = 0.5f;

    
    void Start()
    {
        upgradable = GetComponent<Upgradable>();
        curHealth = upgradable.GetCurGradeAttributes().healthPoints;
        upgradable.onUpgrade += OnUpgrade;
    }


    public void TakeDamage(float damage)
    {
        curHealth -= damage;
        if (curHealth/upgradable.GetCurGradeAttributes().healthPoints < crackedPercentage)
        {
            upgradable.Cracked();
        }
        if (curHealth <= 0)
        {
            upgradable.ReduceToGrade(0);
            wallCollider.enabled = false;
        }

    }

    public void OnUpgrade()    
    {
        curHealth = upgradable.GetCurGradeAttributes().healthPoints;
        wallCollider.enabled = true;
    }


}
