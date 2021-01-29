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
        [SerializeField] private SheepSettings sheepSettings;
        [SerializeField] private Image collectionDisplay;
        [SerializeField] private ParticleSystem collectionParticle;
        [SerializeField] private ParticleSystem sleepingParticle;
        [SerializeField] private GameEvent onSheepDeath;
        
        [Header("Light Settings")]
        [SerializeField] private Vector2 intensityRange = new Vector2(.3f, .6f);
        [SerializeField] private float flickerSpeed = 5f;
        private Vector2 noiseVar;

        private Animator animator;
        private SpriteRenderer sr;
        private Selectable selectable;
        private Light2D sheepLight;
        private float health;
        private Status status;

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
            noiseVar = new Vector2(Random.value, Random.value);

            animator = GetComponent<Animator>();
            sr = GetComponent<SpriteRenderer>();
            if (sr == null) sr = GetComponentInParent<SpriteRenderer>();
            selectable = GetComponent<Selectable>() ?? GetComponentInChildren<Selectable>();
            sheepLight = GetComponentInChildren<Light2D>();
            sheepLight.enabled = false;
            PlayerController.PlayerSettings.onManaChange.Register(gameObject, RefreshSelectableInteractable);
            
            waitWhileShearing = new WaitForSeconds(sheepSettings.fillTime);

        }
        
        private void Update()
        {
            if (status.HasFlag(Status.Empty)) return;
            if ((status & Status.Glow) != 0)
            {
                var noise = Mathf.PerlinNoise(Time.time*flickerSpeed + noiseVar.x, noiseVar.y);
                sheepLight.intensity = Mathf.Lerp(intensityRange.x, intensityRange.y, noise);
            }
            if (selectable.DragTime < 0) collectionParticle.Stop();
            if (selectable.DragTime >= 0) DisplayBeingCollected();
            else collectionDisplay.fillAmount = 0;
            if (selectable.DragTime >= sheepSettings.timeToCollect) GetCollected();
        }

        private void RefreshSelectableInteractable(object o)
        {
            if (selectable.enabled && PlayerMaxMana) selectable.enabled = false;
            if (!selectable.enabled && !PlayerMaxMana) selectable.enabled = true & (status & Status.Glow) != 0;
        }

        private void OnEnable()
        {
            sheepSettings.sheeps.Add(this);
            
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

        private void GetCollected()
        {
            Debug.Log(gameObject.name);
            PlayerController.PlayerSettings.UpdateMana(sheepSettings.manaAddition);
            status |= Status.Empty;
            RefreshSprite();
            collectionParticle.Stop();
            collectionDisplay.fillAmount = 0;
            selectable.enabled = false;
            StartCoroutine(WaitWhileShearing());
        }
        

        IEnumerator WaitWhileShearing()
        {
            yield return waitWhileShearing;
            Refill();
        }
        

        public void Refill()
        {
            status &= ~Status.Empty;
            RefreshSprite();
        }

        private void DisplayBeingCollected()
        {
            collectionDisplay.fillAmount = selectable.DragTime / sheepSettings.timeToCollect;
            if (!collectionParticle.isPlaying) collectionParticle.Play();
        }

        private void RefreshSprite()
        {
            animator.SetInteger(StatusAnimatorID, (int) status);
            
            sheepLight.enabled = false;
            if ((status & Status.Awake) == 0) sleepingParticle.Play();
            else sleepingParticle.Stop();
            if ((status & Status.Empty) == 0 && (status & Status.Glow) != 0) sheepLight.enabled = true;

            selectable.enabled = (status & Status.Glow) != 0 && (status & Status.Empty) == 0;
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
            if (health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            onSheepDeath.Raise();
            gameObject.SetActive(false);
        }
    }
}