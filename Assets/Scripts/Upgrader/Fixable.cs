using System;
using Enemies;
using Player;
using UnityEngine;

namespace Upgrader
{
    public class Fixable : MonoBehaviour, IEnemyDamage
    {
        public Action onDeath, onHalfHealth, onFixed, onHealthChange;
        [SerializeField] private float curHealth;
        [SerializeField] private float crackedPercentage = .5f;
        private Animator anim;

        private float maxHealth;
        private int maxRequiredWoods;
        private int maxRequiredRocks;
        private bool halfHealthEventThrew;
        private InventoryObject Inventory => PlayerController.CurrentInventory;
        public bool ShouldFix => curHealth < maxHealth;
        public int RequiredWood => Mathf.CeilToInt((1 - curHealth / maxHealth) * maxRequiredWoods);
        public int RequiredRock => Mathf.CeilToInt((1 - curHealth / maxHealth) * maxRequiredRocks);

        private void Start()
        {
            anim = GetComponent<Animator>();

        }

        public void SetUp(float health, int requiredWood, int requiredRock)
        {
            maxHealth = health;
            curHealth = maxHealth;
            maxRequiredRocks = requiredRock;
            maxRequiredWoods = requiredWood;
            halfHealthEventThrew = false;
        }

        public void Fix()
        {
            Inventory[ResourceType.Wood] -= RequiredWood;
            Inventory[ResourceType.Rock] -= maxRequiredRocks;
            curHealth = maxHealth;
            onFixed?.Invoke();
            halfHealthEventThrew = false;
        }
    
        public void TakeDamage(float damage)
        {
            anim.SetBool("IsAttacked", true);
            onHealthChange?.Invoke();
            curHealth -= damage;
            if (curHealth / maxHealth <= crackedPercentage && !halfHealthEventThrew)
            {
                onHalfHealth?.Invoke();
                halfHealthEventThrew = true;
            }

            if (curHealth <= 0)
                onDeath?.Invoke();
        }

        public void DoneAttack()
        {
            anim.SetBool("IsAttacked", false);
        }

        // private static int i;
        // public int index;
        //
        // private void Start()
        // {
        //     index = i++;
        // }
        //
        // private void OnGUI()
        // {
        //     GUILayout.Space(30f+20f*index);
        //     if (GUILayout.Button("damage"))
        //     {
        //         TakeDamage(maxHealth / 4);
        //     }
        // }
    }
}
