using System;
using System.Collections;
using Pathfinding;
using UnityEngine;

namespace Enemies
{
    public enum Mode {Walking, Attacking}
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemySettings enemySettings;

        private IEnemyDamage attackedObject;
        // private bool shouldChangeTarget;
        // private bool canAttack = true;
        private EnemyMovement enemyMovement;

        private Mode mode;
        private IEnemyDamage currentAttacked;

        void Start()
        {
            enemyMovement = GetComponent<EnemyMovement>();
            // InvokeRepeating(nameof(UpdateTarget), enemy
            // Settings.changeTargetRate, enemySettings.changeTargetRate);
            mode = Mode.Walking;
        }

        private void Update()
        {
            switch (mode)
            {
                case Mode.Attacking:
                    //animation//
                    break;
                case Mode.Walking:
                    //animation
                    enemyMovement.Move();
                    break;
            }
        }

        // updates the target every "changeTargetRate" time according to the nearest target to the enemy.
        // private void UpdateTarget()
        // {
        //     var wallsDistance = Vector2.Distance(enemyMovement.wallsPosition.position,
        //         enemyMovement.rb.position);
        //     var playerDistance = Vector2.Distance(enemyMovement.playerTransform.position,
        //         enemyMovement.rb.position);
        //     if (enemySettings.target.position == enemyMovement.playerTransform.position &&
        //         wallsDistance < playerDistance)
        //     {
        //         enemySettings.target = enemyMovement.wallsPosition;
        //         if (enemySettings.enemyMode != Mode.AttackPlayer)
        //         {
        //             enemySettings.enemyMode = Mode.AttackWall;
        //         }
        //     }
        //     else if (playerDistance < wallsDistance)
        //     {
        //         enemySettings.target = enemyMovement.playerTransform;
        //         if (enemySettings.enemyMode != Mode.AttackWall)
        //         {
        //             enemySettings.enemyMode = Mode.AttackPlayer;
        //         }
        //     }
        // }

        // attacks a damageable target.
        private void Attack()
        {
            currentAttacked.TakeDamage(enemySettings.attackPower);
        }

        // delays the attack rate.
        // private IEnumerator DelayAttack()
        // {
        //     yield return new WaitForSeconds(enemySettings.attackDelay);
        //     canAttack = true;
        // }

        // sets the target to a target that enemy collided with.
        // private void SetCollisionTarget(Transform newTarget, Mode mode, IDamageable toAttack)
        // {
        //     enemySettings.enemyMode = mode;
        //     enemySettings.target = newTarget;
        //     attackedObject = toAttack;
        //     shouldChangeTarget = false;
        // }

        public void TakeDamage(float damage)
        {
            enemySettings.UpdateLife(damage);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (mode == Mode.Attacking) return;
            var hit = other.gameObject.GetComponent<IEnemyDamage>();
            if (hit == null) return;
            mode = Mode.Attacking;
            currentAttacked = hit;
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.GetComponent<IEnemyDamage>() == currentAttacked)
            {
                currentAttacked = null;
                mode = Mode.Walking;
            }
        }
    }
}