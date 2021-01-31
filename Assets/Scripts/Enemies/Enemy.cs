using System;
using System.Collections;
using Cycles;
using DG.Tweening;
using Events;
using Pathfinding;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Enemies
{
    public enum Mode {Walking, Attacking, Dying}
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemySettings enemySettings;
        [SerializeField] private GameObject enemyGFX;
        [SerializeField] private float stuckTimeThreshold = 2.5f;
        [SerializeField] private Shader dieShader;
        private Material dieMaterial;
        [SerializeField] private Light2D light2D;
        
        private Material defaultMaterial;
        private float dieMaterialEdge = 0f;
        private SpriteRenderer spriteRenderer;
        private SoundController soundController;
        private Mode mode;
        private IEnemyDamage currentAttacked;
        private float curHealth;
        private Collider2D enemyCollider;
        private BoxCollider2D bulletCollider;
        
        private AIPath aiPath;
        private Vector3 gfxScale;
        private Tween tween;
        private Animator animator;
        private Rigidbody2D rb;
        private RigidbodyConstraints2D originalConstraints;
        
        private readonly int attackAnimationID = Animator.StringToHash("Attack");
        private readonly int moveAnimationID = Animator.StringToHash("Move");
        private readonly int dieAnimationID = Animator.StringToHash("Die");

        private float stuckTimer;
        private bool canRoar = true;
        private bool shouldAttack = false;
        private float fadeSpeed = 0.35f;
        private float maxFadeValue = 0.65f;
        
        private void Start()
        {
            soundController = FindObjectOfType<SoundController>();
            GetComponent<Seeker>().graphMask = GraphMask.FromGraphName("Enemy Graph");
            aiPath = GetComponent<AIPath>();
            animator = GetComponent<Animator>();
            mode = Mode.Walking;
            gfxScale = enemyGFX.transform.localScale;
            curHealth = enemySettings.health;
            spriteRenderer = enemyGFX.GetComponent<SpriteRenderer>();
            defaultMaterial = spriteRenderer.material;
            // dieMaterialEdge = dieMaterial.GetFloat("edge");
            dieMaterial = new Material(dieShader);
            bulletCollider = GetComponent<BoxCollider2D>();
            enemyCollider = transform.GetChild(0).GetComponent<Collider2D>();
            CyclesManager.Instance.NightSettings.OnCycleEnd.Register(gameObject, o => Die());

            rb = GetComponent<Rigidbody2D>();
            originalConstraints = rb.constraints;
        }

        private void Update()
        {
            Debug.Log(mode);
            if (mode == Mode.Dying)
            {
                ManageDeath();
                return;
            }
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
                enemyCollider.enabled = false;

                IEnumerator WaitAndEnable()
                {
                    yield return new WaitForSeconds(.5f);
                    enemyCollider.enabled = true;
                }

                StartCoroutine(WaitAndEnable());
            }
        }
        private void AttackMode()
        {
            if (mode == Mode.Attacking || mode == Mode.Dying) return;
            mode = Mode.Attacking;
            animator.SetTrigger(attackAnimationID);
            if(!canRoar) return;
            soundController.PlaySoundEffect(soundController.soundSettings.monsterAttack);
            StartCoroutine(AttackSoundDelay());
        }

        private IEnumerator AttackSoundDelay()
        {
            canRoar = false;
            yield return new WaitForSeconds(soundController.soundSettings.monsterAttack.length * 3f);
            canRoar = true;
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

        // private void LateUpdate()
        // {
        //     if (Input.GetKeyDown(KeyCode.Space))
        //     {
        //         TakeDamage(0);
        //     }
        // }

        public void TakeDamage(float damage)
        {
            curHealth -= damage;
            tween?.Kill(true);
            tween = DOTween.Sequence()
                .Append(DOTween.To(() => light2D.intensity, f => light2D.intensity = f, 0.2f, 0.05f))
                .Append(DOTween.To(() => light2D.intensity, f => light2D.intensity = f, 1.24F, 0.2f).SetDelay(0.1f));
            if (curHealth <= 0)
            {
                curHealth = enemySettings.health;
                Die();
            }
        }

        private void ManageDeath()
        {
            // dieMaterialEdge += Time.deltaTime * enemySettings.fadeSpeed;
            dieMaterialEdge += Time.deltaTime * enemySettings.fadeSpeed;
            dieMaterial.SetFloat("edge", dieMaterialEdge);
            // aiPath.canMove = false;
            // enemyCollider.enabled = false;
            // light2D.intensity = 0;

            if (dieMaterialEdge >= maxFadeValue)
            {
                SetDead();
            }
        }
        
        public void Die()
        {
            mode = Mode.Dying;
            aiPath.canMove = false;
            enemyCollider.enabled = false;
            bulletCollider.enabled = false;
            tween?.Kill();
            light2D.intensity = 0;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            // animator.SetTrigger(dieAnimationID);
            // spriteRenderer.material = dieMaterial;
            spriteRenderer.material = dieMaterial;
        }

        public void SetDead()
        {
            spriteRenderer.material = defaultMaterial;
            mode = Mode.Walking;
            aiPath.canMove = true;
            enemyCollider.enabled = true;
            bulletCollider.enabled = true;
            dieMaterial.SetFloat("edge", 0f);
            dieMaterialEdge = 0f;
            light2D.intensity = 1;
            rb.constraints = originalConstraints;
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
            shouldAttack = true;
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (mode == Mode.Dying) return;
            if (other.gameObject.GetComponent<IEnemyDamage>() != currentAttacked) return;
            if (currentAttacked as Sheep.Sheep != null) gameObject.SetActive(false);
            currentAttacked = null;
            WalkMode();

        }

        // private IEnumerator ChangeToWalkDelay()
        // {
        //     yield return new WaitForSeconds(0.1f);
        //     Debug.Log(shouldAttack);
        //     if (!shouldAttack)
        //     {
        //         currentAttacked = null;
        //         shouldAttack = false;
        //         WalkMode();
        //     }
        // }
    }
}