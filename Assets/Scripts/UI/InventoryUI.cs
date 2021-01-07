using System;
using DG.Tweening;
using Player.Inventory;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private InventoryObject inventory;
        [SerializeField] private TMP_Text woodText;
        [SerializeField] private TMP_Text rockText;
        [SerializeField] private TMP_Text mushroomText;

        private TMP_Text TypeToText(ResourceType type)
        {
            switch (type)
            {
                case ResourceType.Wood:
                    return woodText;
                case ResourceType.Rock:
                    return rockText;
                case ResourceType.Mushroom:
                    return mushroomText;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
            if (!(args is InventoryObject.CollectingArgs tArgs))
            {
                woodText.text = inventory[ResourceType.Wood].ToString();
                rockText.text = inventory[ResourceType.Rock].ToString();
                mushroomText.text = inventory[ResourceType.Mushroom].ToString();
            }
            else
            {
                var type = tArgs.type;
                var tmpText = TypeToText(type);
                DOTween.Kill(type, true);
                tmpText.text = inventory[type].ToString();
                if (tArgs.isIncreasing > 0)
                    tmpText.gameObject.transform.DOScale(1.2f * Vector3.one, .25f).SetLoops(2, LoopType.Yoyo).SetId(type);
                else if (tArgs.isIncreasing < 0)
                    tmpText.gameObject.transform.DOScaleY(.8f, .18f).SetLoops(2, LoopType.Yoyo).SetId(type);
            }
        }
    }
}
