using System;
using System.Collections;
using Cycles;
using Enemies;
using Events;
using Player;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Selectable = Selections.Selectable;

namespace Sheep
{
    public class Sheep : MonoBehaviour, IEnemyDamage
    {
        public Selectable ThisSelectable { get; private set; }
        public SheepSettings SheepSettings => sheepSettings;
        
        [SerializeField] private SheepSettings sheepSettings;
        [SerializeField] private ParticleSystem sleepingParticle;
        [SerializeField] private GameEvent onSheepDeath;
        [SerializeField] private GameEvent onShake;

        [Header("Light Settings")]
        [SerializeField] private Vector2 intensityRange = new Vector2(.3f, .6f);
        [SerializeField] private float flickerSpeed = 5f;
        private Vector2 noiseVar;

        private Animator animator;
        private Light2D sheepLight;
        private float health;
        private Status status;
        private bool isDead;

        private static readonly int StatusAnimatorID = Animator.StringToHash("Status");
        private WaitForSeconds waitWhileShearing;

        private bool PlayerMaxMana => PlayerController.PlayerSettings.maxMana - PlayerController.PlayerSettings.curMana < Mathf.Epsilon;
        
        [Flags] private enum Status
        {
            None = 0,
            Awake = 1,
            Empty = 2,
            Glow = 4,
        }

        
        private void Awake()
        {
            animator = GetComponent<Animator>();
            ThisSelectable = GetComponentInChildren<Selectable>();
            sheepLight = GetComponentInChildren<Light2D>();
            
            waitWhileShearing = new WaitForSeconds(sheepSettings.fillTime);
            noiseVar = new Vector2(Random.value, Random.value);
            sheepLight.enabled = false;
            PlayerController.PlayerSettings.onManaChange.Register(gameObject, RefreshSelectableInteractable);
            RandomizeAnimationSpeed();
        }

        private void RandomizeAnimationSpeed() => animator.speed = Random.Range(0.5f, 1f);
        
        private void Update()
        {
            if (status.HasFlag(Status.Empty)) return;
            if ((status & Status.Glow) != 0)
            {
                var noise = Mathf.PerlinNoise(Time.time*flickerSpeed + noiseVar.x, noiseVar.y);
                sheepLight.intensity = Mathf.Lerp(intensityRange.x, intensityRange.y, noise);
            }
        }

        

        private void RefreshSelectableInteractable(object o)
        {
            if (ThisSelectable.enabled && PlayerMaxMana) ThisSelectable.enabled = false;
            if (!ThisSelectable.enabled && !PlayerMaxMana) ThisSelectable.enabled = true & (status & Status.Glow) != 0;
        }

        private void OnEnable()
        {
            sheepSettings.sheeps.Add(this);
            isDead = false;
            health = sheepSettings.maxHealth;
            status = Status.None;
            switch (CyclesManager.Instance.CurrentCycle)
            {
                case CyclesType.Day:
                    status |= Status.Awake;
                    break;
                case CyclesType.Night:
                    break;
                case CyclesType.Eclipse:
                    status |= Status.Glow;
                    status |= Status.Awake;
                    break;
            }
            RefreshSprite();
        }

        private void OnDisable()
        {
            sheepSettings.sheeps.Remove(this);
        }

        public void GetCollected()
        {
            PlayerController.PlayerSettings.UpdateMana(sheepSettings.manaAddition);
            status |= Status.Empty;
            RefreshSprite();
            // collectionParticle.Stop();
            // collectionDisplay.fillAmount = 0;
            ThisSelectable.enabled = false;
            sheepSettings.OnShear.Raise();
            StartCoroutine(WaitWhileShearing());
        }
        

        IEnumerator WaitWhileShearing()
        {
            yield return waitWhileShearing;
            Refill();
        }

        public bool IsSheared => (status & Status.Empty) != 0;
        

        public void Refill()
        {
            status &= ~Status.Empty;
            sheepSettings.OnRefill.Raise();
            RefreshSprite();
        }

        

        private void RefreshSprite()
        {
            animator.SetInteger(StatusAnimatorID, (int) status);
            
            sheepLight.enabled = false;
            if ((status & Status.Awake) == 0) sleepingParticle.Play();
            else sleepingParticle.Stop();
            if ((status & Status.Empty) == 0 && (status & Status.Glow) != 0) sheepLight.enabled = true;

            ThisSelectable.enabled = (status & Status.Glow) != 0 && (status & Status.Empty) == 0;
        }

        public void SetGlow(bool toGlow)
        {
            if (toGlow)
            {
                status |= Status.Glow;
            }
            else
            {
                status &= ~Status.Glow;
            }
            RefreshSprite();
        }

        public void Sleep()
        {
            status &= ~Status.Awake;
            RefreshSprite();
        }

        public void WakeUp()
        {
            status |= Status.Awake;
            RefreshSprite();
        }
        
        public void TakeDamage(float damage)
        {
            health -= damage;
            if (health <= 0 && ! isDead)
            {
                Die();
            }
        }

        private void Die()
        {
            isDead = true;
            onSheepDeath.Raise();
            onShake.Raise();
            gameObject.SetActive(false);
        }
    }
}