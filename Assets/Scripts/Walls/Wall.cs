using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using Upgrader;

public class Wall : MonoBehaviour, IDamageable
{
    [SerializeField] private Collider2D wallCollider;
    private Upgradable upgradable;
    private float curHealth;

    private bool temp = true;
    
    void Start()
    {
        upgradable = GetComponent<Upgradable>();
        curHealth = upgradable.GetCurGradeAttributes().healthPoints;
        upgradable.onUpgrade += OnUpgrade;
    }


    private void Update()
    {
        Debug.Log("cur health update"+curHealth);
        // Debug.Log("grade: "+upgradable.GetCurGradeAttributes().garde);
        // Debug.Log("grade: "+upgradable.GetCurGradeAttributes().healthPoints);


        // //todo: delede function
        // if (Input.GetKeyUp(KeyCode.Space) && temp)
        // {
        //     TakeDamage(10);
        // }
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("cur health before atack"+curHealth);
        // Debug.Log("level: "+upgradable.GetCurGradeAttributes().garde);

        curHealth -= damage;
        if (curHealth <= 0)
        {
            upgradable.ReduceToGrade(0);
            wallCollider.enabled = false;
        }
        Debug.Log("cur health take damage"+curHealth);
        Debug.Log("level: "+upgradable.GetCurGradeAttributes().garde);

    }

    public void OnUpgrade()    
    {
        curHealth = upgradable.GetCurGradeAttributes().healthPoints;
        wallCollider.enabled = true;
        Debug.Log("cur health ON UPGRADE"+curHealth);
    }


}
