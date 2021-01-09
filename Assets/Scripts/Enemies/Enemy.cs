using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private EnemySettings enemySettings;

        private IDamageable attackedObject;
        private bool shouldChangeTarget;
        private bool canAttack = true;
        private EnemyMovement enemyMovement;

        // Start is called before the first frame update
        void Start()
        {
            enemyMovement = GetComponent<EnemyMovement>();
            InvokeRepeating(nameof(UpdateTarget), enemySettings.changeTargetRate, enemySettings.changeTargetRate);
        }

        void FixedUpdate()
        {
            if (enemySettings.enemyMode != Mode.Walking && attackedObject != null)
            {
                Attack(attackedObject);
            }
        }

        // updates the target every "changeTargetRate" time according to the nearest target to the enemy.
        private void UpdateTarget()
        {
            var wallsDistance = Vector2.Distance(enemyMovement.wallsPosition.position,
                enemyMovement.rb.position);
            var playerDistance = Vector2.Distance(enemyMovement.playerTransform.position,
                enemyMovement.rb.position);
            if (enemySettings.target.position == enemyMovement.playerTransform.position &&
                wallsDistance < playerDistance)
            {
                enemySettings.target = enemyMovement.wallsPosition;
                if (enemySettings.enemyMode != Mode.AttackPlayer)
                {
                    enemySettings.enemyMode = Mode.AttackWall;
                }
            }
            else if (playerDistance < wallsDistance)
            {
                enemySettings.target = enemyMovement.playerTransform;
                if (enemySettings.enemyMode != Mode.AttackWall)
                {
                    enemySettings.enemyMode = Mode.AttackPlayer;
                }
            }
        }

        // attacks a damageable target.
        private void Attack(IDamageable damageable)
        {
            if (!shouldChangeTarget && canAttack && Vector2.Distance(enemyMovement.rb.position,
                enemySettings.target.position) <= enemySettings.attackDistance)
            {
                damageable.TakeDamage(enemySettings.attackPower);
                canAttack = false;
                StartCoroutine(DelayAttack());
            }
            else if (shouldChangeTarget)
            {
                enemyMovement.StartFollow();
            }
        }

        // delays the attack rate.
        private IEnumerator DelayAttack()
        {
            yield return new WaitForSeconds(enemySettings.attackDelay);
            canAttack = true;
        }

        // sets the target to a target that enemy collided with.
        private void SetCollisionTarget(Transform newTarget, Mode mode, IDamageable toAttack)
        {
            enemySettings.enemyMode = mode;
            enemySettings.target = newTarget;
            attackedObject = toAttack;
            shouldChangeTarget = false;
        }

        public void TakeDamage(float damage)
        {
            enemySettings.UpdateLife(damage);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                SetCollisionTarget(other.gameObject.transform, Mode.AttackPlayer,
                    other.gameObject.GetComponent<IDamageable>());
            }

            //todo: uncomment the following lines after sheep and wall implement idamageable.
            // else if (other.gameObject.CompareTag("Sheep"))
            // {
            //     SetCollisionTarget(other.gameObject.transform, EnemyMode.AttackSheep,
            //         other.gameObject.GetComponent<IDamageable>());
            // }
            // else if (other.gameObject.CompareTag("Wall"))
            // {
            //     SetCollisionTarget(other.gameObject.transform, EnemyMode.AttackWall,
            //         other.gameObject.GetComponent<IDamageable>());
            // }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Wall") && enemySettings.enemyMode != Mode.AttackWall)
            {
                shouldChangeTarget = true;
            }
            else if (other.gameObject.CompareTag("Player") && enemySettings.enemyMode != Mode.AttackPlayer)
            {
                shouldChangeTarget = true;
            }
            else if (other.gameObject.CompareTag("Sheep") && enemySettings.enemyMode != Mode.AttackSheep)
            {
                shouldChangeTarget = true;
            }
        }
    }
}