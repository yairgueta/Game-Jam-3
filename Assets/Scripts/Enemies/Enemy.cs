using Pathfinding;
using Spawners;
using UnityEngine;

namespace Enemies
{
    public enum Mode {Walking, Attacking}
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemySettings enemySettings;
        [SerializeField] private GameObject enemyGFX;

        private AIPath aiPath;
        private Mode mode;
        private IEnemyDamage currentAttacked;
        private Vector3 gfxScale;

        private Animator animator;
        private readonly int attackAnimationID = Animator.StringToHash("Attack");
        private readonly int moveAnimationID = Animator.StringToHash("Move");

        private void Start()
        {
            GetComponent<Seeker>().graphMask = GraphMask.FromGraphName("Enemy Graph");
            aiPath = GetComponent<AIPath>();
            animator = GetComponent<Animator>();
            mode = Mode.Walking;
            gfxScale = enemyGFX.transform.localScale;
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
                    break;
            }
            ManageDirection();
        }

        private void AttackMode()
        {
            if (mode == Mode.Attacking) return;
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
            enemySettings.UpdateLife(damage);
        }

        private void ManageDirection()
        {
            var gfxTransform = enemyGFX.transform;
            if (aiPath.desiredVelocity.x >= 0.01f)
            {
                gfxTransform.localScale = new Vector3( -gfxScale.x, gfxScale.y, gfxScale.z);
            }
            if (aiPath.desiredVelocity.x <= -0.01f)
            {
                gfxTransform.localScale = new Vector3( gfxScale.x, gfxScale.y, gfxScale.z);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (mode == Mode.Attacking) return;
            var hit = other.gameObject.GetComponent<IEnemyDamage>();
            if (hit == null) return;
            AttackMode();
            currentAttacked = hit;
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.GetComponent<IEnemyDamage>() != currentAttacked) return;
            currentAttacked = null;
            WalkMode();
        }
    }
}