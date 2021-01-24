using System;
using DG.Tweening;
using Player;
using UnityEngine;

namespace Walls
{
    public class Door : MonoBehaviour
    {
        [Header("Animation Attributes")]
        [SerializeField] private Ease easeOpenFunction;
        [SerializeField] private Ease easeCloseFunction;
        [SerializeField] private float openDuration, closeDuration;
        
        [Header("References")]
        [SerializeField] private Transform innerDoor;
        [SerializeField] private float endY;
        private float startY;
        private float overallDistance;

        private Tween tween;
        
        private void Start()
        {
            startY = innerDoor.position.y;
            overallDistance = startY - endY;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<PlayerController>() != null) OpenAnimation();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<PlayerController>() != null) CloseAnimation();
        }


        void OpenAnimation()
        {
            tween?.Kill(false);
            var dur = ((innerDoor.position.y - endY) / overallDistance) * openDuration;
            tween = innerDoor.DOMoveY(endY, dur).SetEase(easeOpenFunction);
        }

        void CloseAnimation()
        {
            tween?.Kill(false);
            var dur = ((startY - innerDoor.position.y) / overallDistance) * closeDuration;
            tween = innerDoor.DOMoveY(startY, dur).SetEase(easeCloseFunction);

        }
    }
}
