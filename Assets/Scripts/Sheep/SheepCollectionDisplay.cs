using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Selectable = Selections.Selectable;

namespace Sheep
{
    public class SheepCollectionDisplay : MonoBehaviour
    {
        [Header("Collection Display")]
        [SerializeField] private Image collectionDisplay;
        [SerializeField] private Image collectionDisplayBG;
        [SerializeField] private ParticleSystem collectionParticle;
        [SerializeField] private float collectionTimeout;
        
        private Sheep sheep;
        private Selectable sheepSelectable;
        private bool hasResetedCollectionDisplay = false;
        private bool isBeingCollectedCoroutinPlaying = false;
        

        private void Start()
        {
            collectionDisplayBG.enabled = false;
            
            sheep = GetComponent<Sheep>();
            sheepSelectable = sheep.ThisSelectable;
            sheepSelectable.onThisMouseEnter += () => collectionDisplayBG.enabled = true;
            sheepSelectable.onThisMouseExit += () => collectionDisplayBG.enabled = false;
            sheepSelectable.onThisSelected += () => StartCoroutine(BeingCollectedCoroutine());
        }
        
        
        private void Update()
        {
            if (sheepSelectable.DragTime < 0 && !hasResetedCollectionDisplay)
            {
                collectionParticle.Stop();
                // collectionDisplayBG.enabled = sheepSelectable.IsMouseOver;
                collectionDisplay.fillAmount = 0;
                hasResetedCollectionDisplay = true;
                return;
            }

            hasResetedCollectionDisplay = false;
            if (sheepSelectable.DragTime >= 0 && !isBeingCollectedCoroutinPlaying) DisplayBeingCollected();
            if (sheepSelectable.DragTime >= sheep.SheepSettings.timeToCollect)
            {
                collectionDisplayBG.enabled = false;
                sheep.GetCollected();
            }
        }

        IEnumerator BeingCollectedCoroutine()
        {
            isBeingCollectedCoroutinPlaying = true;
            var t = 0f;
            var startTime = Time.time;
            while (t < collectionTimeout)
            {
                collectionDisplay.fillAmount = (Time.time - startTime) / sheep.SheepSettings.timeToCollect;
                if (!collectionParticle.isPlaying) collectionParticle.Play();
                yield return null;
                t += Time.deltaTime;
            }

            isBeingCollectedCoroutinPlaying = false;
        }
        
        private void DisplayBeingCollected()
        {
            collectionDisplay.fillAmount = sheepSelectable.DragTime / sheep.SheepSettings.timeToCollect;
            if (!collectionParticle.isPlaying) collectionParticle.Play();
        }
    }
}