using System.Collections;
using System.Collections.Generic;
using Cycles;
using DG.Tweening;
using Events;
using Player;
using UnityEngine;
using Object = System.Object;

public class MinimapIcons : MonoBehaviour
{
    private Animator anim;
    private Vector3 originalScale;
    private Transform sheepTransform;
    private Tween tween;
    [SerializeField] private SpriteRenderer sheepRenderer;
    [SerializeField] private Color attackColor;
    [SerializeField] private Color eclipseColor;
    [SerializeField] private Color originColorSheep;
    [SerializeField] private float toColorDuration;
    [SerializeField] private float fromColorDuration;
    [SerializeField] private GameEvent onWallAttack;
    [SerializeField] private GameEvent onShear;
    [SerializeField] private GameEvent onRefill;
    [SerializeField] private SheepSettings sheepSettings;
    [SerializeField] private Ease scaleEaseIn;

    void Start()
    {
        sheepRenderer = GetComponent<SpriteRenderer>();
        sheepTransform = GetComponent<Transform>();
        originalScale = sheepTransform.localScale;
        onWallAttack.Register(gameObject, StartAttack);
        onRefill.Register(gameObject, OnEclipse);
        onShear.Register(gameObject, OnEclipse);
        CyclesManager.Instance.EclipseSettings.OnCycleEnd.Register(gameObject, DoneEclipse);
        CyclesManager.Instance.EclipseSettings.OnCycleStart.Register(gameObject, OnEclipse);
    }

    public void StartAttack(object o)
    {
        FlashAnimation(attackColor, originColorSheep, 3);
    }
    
    // public void DoneAttack(object o)
    // {
    //     // tween?.Kill();
    // }

    public void OnEclipse(object o)
    {
        if (CyclesManager.Instance.CurrentCycle != CyclesType.Eclipse) return;

        foreach (var sheep in sheepSettings.sheeps)
        {
            if (!sheep.IsSheared)
            {
                Debug.Log("not sheard");
                FlashAnimation(eclipseColor, originColorSheep, -1);
                return;
            }
        }

        Debug.Log("all sheared");
        tween?.Kill(true);
        sheepRenderer.color = originColorSheep;
        sheepTransform.localScale = originalScale;

    }

    private void FlashAnimation(Color targetColor, Color originColor, int loops)
    {
        tween?.Kill(true);
        tween = DOTween.Sequence()
            .Append(sheepRenderer.DOColor(targetColor, toColorDuration).SetEase(scaleEaseIn))
            .Join(sheepTransform.DOScale(originalScale*1.5f,toColorDuration))
            .Append(sheepRenderer.DOColor(originColor, fromColorDuration).SetEase(scaleEaseIn))
            .Join(sheepTransform.DOScale(originalScale,toColorDuration))
            .SetLoops(loops);
    }
    
    public void DoneEclipse(object o)
    {
        tween?.Kill();
        sheepRenderer.color = originColorSheep;
        sheepTransform.localScale = originalScale;
    }
}
