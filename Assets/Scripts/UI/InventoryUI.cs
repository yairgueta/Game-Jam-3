using System;
using DG.Tweening;
using Player;
using UnityEngine;
using TMPro;

namespace UI
{
    public class InventoryUI : MonoBehaviour
    {
        // [SerializeField] private GameObject woodDisplay;
        // [SerializeField] private GameObject rockDisplay;
        [SerializeField] private TMP_Text woodText, rockText;
        
        [SerializeField] private TMP_Text woodAnimationTxt;
        [SerializeField] private TMP_Text rockAnimationTxt;
        private SoundController soundController;
        
        private Tween woodTween;
        private Tween rockTween;

        private void Start()
        {
            soundController = FindObjectOfType<SoundController>();
            UpdateResources(null);
            woodAnimationTxt.text = "";
            rockAnimationTxt.text = "";
        }

        private (TMP_Text, TMP_Text) TypeToText(ResourceType type, int diff)
        {
            switch (type)
            {
                case ResourceType.Wood:
                    woodAnimationTxt.text = diff.ToString();
                    return (woodText, woodAnimationTxt);
                case ResourceType.Rock:
                    rockAnimationTxt.text = diff.ToString();
                    return (rockText, rockAnimationTxt);
            }

            return (null, null);
        }

        public void WarnLackOfResource(object args)
        {
            var t = ((InventoryObject.CollectingArgs) args);
            var (lack, _) = TypeToText(t.type, t.difference);
            
            DOTween.Kill(t, true);
            var dur = .5f;
            DOTween.Sequence()
                .Join(lack.gameObject.transform.DOPunchScale(0.2f * Vector3.one, dur))
                .Join(lack.DOColor(Color.red, dur * 0.5f).SetLoops(2, LoopType.Yoyo))
                .SetId(t);
        }

        private void PlayPickSound(ResourceType type)
        {
            switch (type)
            {
                case ResourceType.Wood:
                    soundController.PlaySoundEffect(soundController.soundSettings.pickWood);
                    return;
                case ResourceType.Rock:
                    soundController.PlaySoundEffect(soundController.soundSettings.pickStone);
                    return;
            }
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
                var difference = tArgs.difference;
                var (tempText, animText) = TypeToText(type, difference);
                if (tempText == null) return; //TODO FIX THIS LINE
                tempText.text = inventory[type].ToString();
                if (tArgs.isIncreasing > 0)
                {
                    PlayPickSound(type);
                    animText.text = "+" + animText.text;
                    AnimateChange(animText, type);
                }
                else if (tArgs.isIncreasing < 0)
                {
                    animText.text = animText.text; // todo: find out why - already happens
                    AnimateChange(animText, type);
                }
            }
            
            // woodDisplay.SetActive(inventory[ResourceType.Wood] != 0);
            // rockDisplay.SetActive(inventory[ResourceType.Rock] != 0);
        }

        private void AnimateChange(TMP_Text animText, ResourceType type)
        {
            animText.gameObject.transform.localScale = Vector3.zero;
            if (type == ResourceType.Wood)
            {
                woodTween.Kill();
                woodTween = DOTween.Sequence().Append(animText.gameObject.transform.DOScale(
                        1.2f * Vector3.one, .25f).SetId(type))
                    .Append(animText.gameObject.transform.DOScale(Vector3.zero, .25f).SetDelay(0.5f));
                return;
            }
            rockTween.Kill();
            rockTween = DOTween.Sequence().Append(animText.gameObject.transform.DOScale(
                    1.2f * Vector3.one, .25f).SetId(type))
                .Append(animText.gameObject.transform.DOScale(Vector3.zero, .25f).SetDelay(0.5f));
        }

    }
}
