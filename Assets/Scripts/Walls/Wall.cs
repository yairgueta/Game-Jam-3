using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Events;
using UnityEngine;
using Upgrader;

public class Wall : MonoBehaviour
{
    [SerializeField] private Collider2D wallCollider;
    private Upgradable upgradable;
    private Fixable fixable;
    [SerializeField] private GameEvent onWallDestroy;

    
    void Start()
    {
        upgradable = GetComponent<Upgradable>();
        fixable ??= GetComponent<Fixable>() ?? GetComponentInChildren<Fixable>();
        OnUpgrade();
        upgradable.onUpgrade += OnUpgrade;
        fixable.onWallBreak += OnBreak;
    }

    private void OnBreak()
    {
        onWallDestroy.Raise();
        upgradable.ReduceToGrade(0);
        wallCollider.enabled = false;
    }
    

    public void OnUpgrade()    
    {
        var gradeAttributes = upgradable.GetCurGradeAttributes();
        var prevGrade = upgradable.GetPreviousGradeAttributes();
        
        fixable.SetUp(gradeAttributes.healthPoints, upgradable.GetCompleteSprite(), upgradable.GetCrackedSprite(),
            prevGrade.requiredWoods, prevGrade.requiredRocks);
        wallCollider.enabled = true;
    }


}
