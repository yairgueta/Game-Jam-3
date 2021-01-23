using System;
using Enemies;
using Player;
using UnityEngine;

namespace Upgrader
{
    public class Fixable : MonoBehaviour, IEnemyDamage
    {
        public Action onDeath, onHalfHealth, onFixed;
        [SerializeField] private float curHealth;
        [SerializeField] private float crackedPercentage = .5f;
        
        private float maxHealth;
        private int maxRequiredWoods;
        private int maxRequiredRocks;
        private bool halfHealthEventThrew = false;
        private InventoryObject Inventory => PlayerController.CurrentInventory;
        public bool ShouldFix => curHealth < maxHealth && curHealth > 0;
        public int RequiredWood => Mathf.FloorToInt((curHealth / maxHealth) * maxRequiredWoods);
        public int RequiredRock => Mathf.FloorToInt((curHealth / maxHealth) * maxRequiredRocks);
        
        
        public void SetUp(float health, int requiredWood, int requiredRock)
        {
            maxHealth = health;
            curHealth = maxHealth;
            maxRequiredRocks = requiredRock;
            maxRequiredWoods = requiredWood;
        }

        public void Fix()
        {
            Inventory[ResourceType.Wood] -= RequiredWood;
            Inventory[ResourceType.Rock] -= maxRequiredRocks;
            curHealth = maxHealth;
            halfHealthEventThrew = false;
        }
    
        public void TakeDamage(float damage)
        {
            curHealth -= damage;
            if (curHealth / maxHealth <= crackedPercentage && !halfHealthEventThrew)
            {
                onHalfHealth?.Invoke();
                halfHealthEventThrew = true;
            }
            if (curHealth <= 0)
                onDeath?.Invoke();
        }

        public static int i = 0;
        public int index;

        private void Start()
        {
            index = i++;
        }

        private void OnGUI()
        {
            GUILayout.Space(30f+20f*index);
            if (GUILayout.Button("damage"))
            {
                TakeDamage(maxHealth / 4);
            }
        }
    }
}
