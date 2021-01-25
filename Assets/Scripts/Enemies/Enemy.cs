using System;
using System.Collections;
using Cycles;
using Events;
using Pathfinding;
using UnityEngine;

namespace Enemies
{
    public enum Mode {Walking, Attacking, Dying}
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemySettings enemySettings;
        [SerializeField] private GameObject enemyGFX;
        [SerializeField] private float stuckTimeThreshold = 2.5f;
        private SoundController soundController;
        private Mode mode;
        private IEnemyDamage currentAttacked;
        private float curHealth;

        private Collider2D collider;
        
        private AIPath aiPath;
        private Vector3 gfxScale;

        private Animator animator;
        private readonly int attackAnimationID = Animator.StringToHash("Attack");
        private readonly int moveAnimationID = Animator.StringToHash("Move");
        private readonly int dieAnimationID = Animator.StringToHash("Die");

        private float stuckTimer;
        
        
        private void Start()
        {
            soundController = FindObjectOfType<SoundController>();
            GetComponent<Seeker>().graphMask = GraphMask.FromGraphName("Enemy Graph");
            aiPath = GetComponent<AIPath>();
            animator = GetComponent<Animator>();
            mode = Mode.Walking;
            gfxScale = enemyGFX.transform.localScale;
            curHealth = enemySettings.health;

            collider = transform.GetChild(0).GetComponent<Collider2D>();
            CyclesManager.Instance.NightSettings.OnCycleEnd.Register(gameObject, o => Die());
        }

        private void Update()
        {
            switch (mode)
            {
                case Mode.Attacking:
                    aiPath.canMove = false;
                    aiPath.canSearch = false;
                    if (currentAttacked == null) WalkMode();
                    break;
                case Mode.Walking:
                    aiPath.canMove = true;
                    aiPath.canSearch = true;
                    ManageStuck();
                    break;
                case Mode.Dying:
                    break;
            }
            ManageDirection();
        }

        // private void OnEnable()
        // {
        //     mode = Mode.Walking;
        // }

        private void ManageStuck()
        {
            if (aiPath.velocity.sqrMagnitude > .1)
            {
                stuckTimer = 0;
            }
            else stuckTimer += Time.deltaTime;

            if (stuckTimer > stuckTimeThreshold)
            {
                collider.enabled = false;

                IEnumerator WaitAndEnable()
                {
                    yield return new WaitForSeconds(.5f);
                    collider.enabled = true;
                }

                StartCoroutine(WaitAndEnable());
            }
        }
        private void AttackMode()
        {
            if (mode == Mode.Attacking) return;
            soundController.PlaySoundEffect(soundController.soundSettings.monsterAttack);
            mode = Mode.Attacking;
            animator.SetTrigger(attackAnimationID);
        }

        private void WalkMode()
        {
            if (mode == Mode.Walking) return;
            mode = Mode.Walking;
            animator.SetTrigger(moveAnimationID);
        }
        
        private void Attack()
        {
            currentAttacked?.TakeDamage(enemySettings.attackPower);
        }

        public void TakeDamage(float damage)
        {
            curHealth -= damage;
            if (curHealth <= 0)
            {
                curHealth = enemySettings.health;
                Die();
            }
        }

        public void Die()
        {
            mode = Mode.Dying;
            animator.SetTrigger(dieAnimationID);
        }

        public void SetDead()
        {
            gameObject.SetActive(false);
        }

        private void ManageDirection()
        {
            var gfxTransform = enemyGFX.transform;
            if (aiPath.desiredVelocity.x >= 0.01f)
            {
                transform.localScale = new Vector3( -1, 1, 1);
            }
            if (aiPath.desiredVelocity.x <= -0.01f)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (mode == Mode.Attacking || mode == Mode.Dying) return;
            var hit = other.gameObject.GetComponent<IEnemyDamage>();
            if (hit == null) return;
            AttackMode();
            currentAttacked = hit;
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (mode == Mode.Dying) return;
            if (other.gameObject.GetComponent<IEnemyDamage>() != currentAttacked) return;
            if (currentAttacked as Sheep.Sheep != null) gameObject.SetActive(false);
            currentAttacked = null;
            WalkMode();
        }
    }
}