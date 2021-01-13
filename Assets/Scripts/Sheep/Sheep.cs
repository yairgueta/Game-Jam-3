using System;
using System.Collections;
using Selections;
using UnityEngine;
using UnityEngine.UI;
using Selectable = Selections.Selectable;

namespace Player
{
    public class Sheep : MonoBehaviour
    {
        [SerializeField] private SheepSettings sheepSettings;
        [SerializeField] private Image collectionDisplay;
        private SpriteRenderer sr;
        private Selectable selectable;

        
        [Flags]
        private enum Status
        {
            None = 0,
            Awake = 1,
            Empty = 2,
            CanShear = 3,
        }

        private Status status;
        
        private void Start()
        {
            sr = GetComponent<SpriteRenderer>();
            selectable = GetComponent<Selectable>();
            status |= Status.Awake;
        }

        private void Update()
        {
            
            if (status.HasFlag(Status.Empty)) return;
            if (selectable.DragTime >= 0) DisplayBeingCollected();
            else collectionDisplay.fillAmount = 0;
            if (PlayerController.PlayerSettings.maxMana - PlayerController.PlayerSettings.curMana < Mathf.Epsilon)
            {
                return;
            }
            if (selectable.DragTime >= sheepSettings.timeToCollect) GetCollected();

        }
        
        private void OnEnable()
        {
            sheepSettings.sheeps.Add(this);
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
            collectionDisplay.fillAmount = 0;
            selectable.SetInteractable(false);
            // Invoke(nameof(Refill), sheepSettings.fillTime);
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
            selectable.SetInteractable(true);
        }

        private void DisplayBeingCollected()
        {
            collectionDisplay.fillAmount = selectable.DragTime / sheepSettings.timeToCollect;
        }

        private void RefreshSprite()
        {
            switch (status)
            {
                case Status.Awake | Status.Empty:
                    sr.sprite = sheepSettings.emptyAwake;
                    break;
                case Status.Awake:      // Awake & Full
                    sr.sprite = sheepSettings.awake;
                    break;
                case Status.Empty :     // Sleep & Empty
                    sr.sprite = sheepSettings.emptySleep;
                    break;
                default:                // Sleep & Full
                    sr.sprite = sheepSettings.sleep;
                    break;
            }
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
    }
}