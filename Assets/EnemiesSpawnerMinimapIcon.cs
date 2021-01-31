using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spawners.EnemiesSpawner;
using UnityEngine;

public class EnemiesSpawnerMinimapIcon : MonoBehaviour
{

    [SerializeField] private int loopsCount;
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
    }

    public void AnimateEnemiesWarning()
    {
        tween?.Kill(true);
        tween = DOTween.Sequence()
            .Join(spriteRenderer.DOColor(warningColor, duration).SetEase(scaleEaseIn))
            .Join(transform.DOScale(originalScale * scaleMultiplier, duration).SetEase(scaleEaseIn))
            .Append(spriteRenderer.DOColor(originalColor, duration).SetEase(scaleEaseOut))
            .Join(transform.DOScale(originalScale, duration).SetEase(scaleEaseOut))
            .SetLoops(loopsCount);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) AnimateEnemiesWarning();
    }
}
