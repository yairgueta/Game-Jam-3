using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;
using Player.Inventory;

public class Upgrader : MonoBehaviour
{
    [SerializeField] private InventoryObject inventory;
    [SerializeField] private WallLevel[] wallLevels;

    public GameEvent onWallSelection;
    public GameEvent onWallUpgraded;
    public GameEvent onWoodLack;
    public GameEvent onRockLack;
   
    private int curLevel;


    private void Start()
    {
        curLevel = 0;
    }

    public WallLevel GetCurWallLeve()
    {
        return wallLevels[curLevel];
    }

    public void OnSelection(object args)
    {
        GameObject selectedObject = (GameObject) args;
        if (selectedObject==gameObject)
        {
            onWallSelection.Raise(gameObject);
        }
    }

    public bool CheckIfCanUpgrade()
    {
        int requiredWood = wallLevels[curLevel].requiredWoods;
        int requiredRocks = wallLevels[curLevel].requiredRocks;
        
        if (inventory[ResourceType.Wood] < requiredWood)
        {
            onWoodLack.Raise();
            return false;
        }
        
        if (inventory[ResourceType.Rock] < requiredRocks)
        {
            onRockLack.Raise();
            return false;
        }

        return true;
    }
    
    public void UpgradeWall()
    {
        int requiredWood = wallLevels[curLevel].requiredWoods;
        int requiredRocks = wallLevels[curLevel].requiredRocks;
        
        inventory[ResourceType.Wood] -= requiredWood;
        inventory[ResourceType.Rock] -= requiredRocks;
        onWallUpgraded.Raise(gameObject);
        curLevel++;
    }
    
    

}
