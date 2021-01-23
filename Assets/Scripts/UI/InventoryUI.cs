using System;
using System.Collections;
using DG.Tweening;
using Player;
using UnityEngine;
using TMPro;

namespace UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private GameObject woodDisplay;
        [SerializeField] private GameObject rockDisplay;
        
        [SerializeField] private TMP_Text woodAnimationTxt;
        [SerializeField] private TMP_Text rockAnimationTxt;
        
        
        private TMP_Text woodText, rockText;

        private void Awake()
        {
            woodText = woodDisplay.GetComponentInChildren<TMP_Text>();
            rockText = rockDisplay.GetComponentInChildren<TMP_Text>();
        }


        
        private TMP_Text TypeToText(ResourceType type)
        {
            switch (type)
            {
                case ResourceType.Wood:
                    return woodText;
                case ResourceType.Rock:
                    return rockText;
            }

            return null;
        }
        
        private TMP_Text TypeToAnimationText(ResourceType type)
        {
            switch (type)
            {
                case ResourceType.Wood:
                    return woodAnimationTxt;
                case ResourceType.Rock:
                    return rockAnimationTxt;
            }

            return null;
        }
        
        public void WarnLackOfResource(object args)
        {
            var t = ((InventoryObject.CollectingArgs) args).type;
            var lack = TypeToText(t);
            
            DOTween.Kill(t, true);
            var dur = .5f;
            DOTween.Sequence()
                .Join(lack.gameObject.transform.DOPunchScale(0.2f * Vector3.one, dur))
                .Join(lack.DOColor(Color.red, dur * 0.5f).SetLoops(2, LoopType.Yoyo))
                .SetId(t);
        }

        public void UpdateResources(object args)
        {
            var inventory = PlayerController.CurrentInventory;
            if (!(args is InventoryObject.CollectingArgs tArgs))
            {
                woodText.text = inventory[ResourceType.Wood].ToString();
                rockText.text = inventory[ResourceType.Rock].ToString();
            }
            else
            {
                var type = tArgs.type;
                var animText = TypeToAnimationText(type);
                var tempText = TypeToText(type);
                if (animText == null) return; //TODO FIX THIS LINE
                DOTween.Kill(type, true);
                tempText.text = inventory[type].ToString();
                animText.text = (Mathf.Abs(int.Parse(tempText.text) - inventory[type])).ToString();
                DOTween.KillAll(animText);
                animText.gameObject.transform.DOScale(1f, 0f);
                if (tArgs.isIncreasing > 0)
                {
                    animText.text = "+" + animText.text;
                    tempText.gameObject.transform.DOScale(1.2f * Vector3.one, .25f).SetLoops(2, LoopType.Yoyo)
                        .SetId(type);
                    StartCoroutine(TextFade(animText));
                }
                else if (tArgs.isIncreasing < 0)
                {
                    animText.text = "-" + animText.text;
                    tempText.gameObject.transform.DOScaleY(.8f, .18f).SetLoops(2, LoopType.Yoyo).SetId(type);
                    StartCoroutine(TextFade(animText));
                }
            }
            
            // woodDisplay.SetActive(inventory[ResourceType.Wood] != 0);
            // rockDisplay.SetActive(inventory[ResourceType.Rock] != 0);
        }

        private IEnumerator TextFade(TMP_Text text)
        {
            yield return new WaitForSeconds(1f);
            text.gameObject.transform.DOScale(0f, 1f);
        }
    }
}
