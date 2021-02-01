using DG.Tweening;
using Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MoreSheepUI : MonoBehaviour
    {
        [SerializeField] private float enterDuration, exitDuration, onScreenDuration;
        [SerializeField] private GameEvent mushroomCollectedEvent;
        [SerializeField] private Ease ease;
        private Tween tween;
        private Image image;
        private RectTransform rt;

        private void Start()
        {
            image = GetComponent<Image>();
            rt = GetComponent<RectTransform>();
            image.color = new Color(255, 255, 255, 0);
            var listener = gameObject.AddComponent<GameEventListener>();
            listener.InitEvent(mushroomCollectedEvent);
            listener.response.AddListener(o => OnSheepCollectedAnimation());
        }

        public void OnSheepCollectedAnimation()
        {
            tween?.Kill(true);
            image.color = Color.white;
            tween = DOTween.Sequence()
                .Append(rt.DOAnchorPosY(0, enterDuration).From().SetEase(ease))
                .Join(image.DOFade(0, enterDuration).From().SetEase(ease))
                .Append(image.DOFade(0f, exitDuration).SetDelay(onScreenDuration));
        }
    }
}
