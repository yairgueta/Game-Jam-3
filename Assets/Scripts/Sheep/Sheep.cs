using System;
using System.Collections;
using Cycles;
using Enemies;
using Player;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;
using Selectable = Selections.Selectable;

namespace Sheep
{
    public class Sheep : MonoBehaviour, IEnemyDamage
    {
        [SerializeField] private SheepSettings sheepSettings;
        [SerializeField] private Image collectionDisplay;
        [SerializeField] private ParticleSystem collectionParticle;
        private SpriteRenderer sr;
        private Selectable selectable;
        private Light2D light;
        private float health;
        private bool PlayerMaxMana => PlayerController.PlayerSettings.maxMana - PlayerController.PlayerSettings.curMana < Mathf.Epsilon;
        
        [Flags]
        private enum Status
        {
            None = 0,
            Awake = 1,
            Empty = 2,
            Glow = 4,
        }

        [SerializeField] private Status status;
        
        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
            if (sr == null) sr = GetComponentInParent<SpriteRenderer>();
            selectable = GetComponent<Selectable>() ?? GetComponentInChildren<Selectable>();
            light = GetComponentInChildren<Light2D>();
            PlayerController.PlayerSettings.onManaChange.Register(gameObject, RefreshSelectableInteractable);
        }
        
        private void Update()
        {
            if (status.HasFlag(Status.Empty)) return;
            
            if (selectable.DragTime < 0) collectionParticle.Stop();
            if (selectable.DragTime >= 0) DisplayBeingCollected();
            else collectionDisplay.fillAmount = 0;
            if (selectable.DragTime >= sheepSettings.timeToCollect) GetCollected();
        }

        private void RefreshSelectableInteractable(object o)
        {
            if (selectable.Interactable && PlayerMaxMana) selectable.SetInteractable(false);
            if (!selectable.Interactable && !PlayerMaxMana) selectable.SetInteractable(true & (status & Status.Glow) != 0);
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
            PlayerController.PlayerSettings.UpdateMana(sheepSettings.manaAddition);
            status |= Status.Empty;
            RefreshSprite();
            collectionParticle.Stop();
            collectionDisplay.fillAmount = 0;
            selectable.SetInteractable(false);
            StartCoroutine(WaitWhileShearing());
        }
        

        IEnumerator WaitWhileShearing()
        {
            yield return new WaitForSeconds(sheepSettings.fillTime);
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
            Sprite sprite;
            light.enabled = false;
            if ((status & Status.Empty) != 0)
            {
                if ((status & Status.Awake) != 0) sprite = sheepSettings.emptyAwake;
                else sprite = sheepSettings.emptySleep;
            }
            else
            {
                if ((status & Status.Awake) != 0) sprite = sheepSettings.awake;
                else sprite = sheepSettings.sleep;
                if ((status & Status.Glow) != 0)
                {
                    sprite = sheepSettings.glowSheep;
                    light.enabled = true;
                }
            }

            sr.sprite = sprite;
            selectable.SetInteractable((status & Status.Glow) != 0);
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
            gameObject.SetActive(false);
        }
    }
}