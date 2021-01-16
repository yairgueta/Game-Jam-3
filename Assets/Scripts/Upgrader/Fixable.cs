using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Player;
using Player.Inventory;
using UnityEngine;
using Upgrader;

public class Fixable : MonoBehaviour, IEnemyDamage
{
    private InventoryObject inventory;
    public Action onWallBreak;
    private SpriteRenderer sr;
    private Sprite completeSprite;
    private Sprite crackedSprite;
    private float maxHealth;
    [SerializeField] private float curHealth;
    private int requiredWoods;
    private int requiredRocks;


    [SerializeField] private float crackedPercentage = 0.5f;

    private void Start()
    {
        inventory = PlayerController.CurrentInventory;
        sr = GetComponent<SpriteRenderer>();
    }

    public Sprite GetCompleteSprite()
    {
        return completeSprite;
    }
    public void SetUp(float health, Sprite complete, Sprite cracked, int requiredWood, int requiredRock)
    {
        maxHealth = health;
        curHealth = maxHealth;
        completeSprite = complete;
        crackedSprite = cracked;
        requiredRocks = requiredRock;
        requiredWoods = requiredWood;
    }
    
    private void Cracked()
    {
        sr.sprite = crackedSprite;
    }

    public bool ShouldFix()
    {
        return curHealth < maxHealth;
    }

    public int RequiredRocksPercentage()
    {
        var percentage = curHealth / maxHealth;
        return (int)(percentage * requiredRocks);
    }
    
    public int RequiredWoodsPercentage()
    {
        var percentage = curHealth / maxHealth;
        return (int)(percentage * requiredWoods);
    }

    public void Fix()
    {
        inventory[ResourceType.Wood] -= RequiredWoodsPercentage();
        inventory[ResourceType.Rock] -= RequiredRocksPercentage();
        sr.sprite = completeSprite;
        curHealth = maxHealth;
    }
    
    public void TakeDamage(float damage)
    {
        curHealth -= damage;
        if (curHealth/maxHealth< crackedPercentage)
        {
            Cracked();
        }
        if (curHealth <= 0)
        {
            onWallBreak?.Invoke();
        }
    }

}
