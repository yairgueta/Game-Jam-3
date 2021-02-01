using Cycles;
using DG.Tweening;
using UnityEngine;

namespace Spawners.EnemiesSpawner
{
    public class EnemiesSpawnerMinimapIcon : MonoBehaviour
    {
        [SerializeField] private float duration;
        [SerializeField] private Color warningColor;
        [SerializeField] private float scaleMultiplier;
        [SerializeField] private Ease scaleEaseIn;
        [SerializeField] private Ease scaleEaseOut;
    
    
        private Tween tween;
        private SpriteRenderer spriteRenderer;
        private Vector3 originalScale;
        private Color originalColor;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            originalScale = transform.localScale;
            originalColor = spriteRenderer.color;
            CyclesManager.Instance.DaySettings.OnCycleEnd.Register(gameObject, o => tween?.Kill(true));
        }

        public void AnimateEnemiesWarning()
        {
            tween?.Kill(true);
            tween = DOTween.Sequence()
                .Join(spriteRenderer.DOColor(warningColor, duration).SetEase(scaleEaseIn))
                .Join(transform.DOScale(originalScale * scaleMultiplier, duration).SetEase(scaleEaseIn))
                .Append(spriteRenderer.DOColor(originalColor, duration).SetEase(scaleEaseOut))
                .Join(transform.DOScale(originalScale, duration).SetEase(scaleEaseOut))
                .SetLoops(-1);
            tween.onKill += () =>
            {
                spriteRenderer.color = originalColor;
                transform.localScale = originalScale;
            };
        }
    }
}
