using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Events;
using UnityEngine;
using UnityEngine.UI;

public class MoreSheepUI : MonoBehaviour
{
    [SerializeField] private float enterDuration, exitDuration, onScreenDuration, f;
    [SerializeField] private GameEvent mushroomCollectedEvent;
    private Tween tween;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        var listener = gameObject.AddComponent<GameEventListener>();
        listener.InitEvent(mushroomCollectedEvent);
        listener.response.AddListener(o => OnSheepCollectedAnimation());
    }

    public void OnSheepCollectedAnimation()
    {
        tween?.Kill(true);
        image.color = Color.white;
        tween = DOTween.Sequence()
            .Append(gameObject.transform.DOMoveY(f, enterDuration).From().SetEase(Ease.OutBounce))
            .Append(image.DOFade(0f, exitDuration).SetDelay(onScreenDuration));
    }
    
    
    
    
}
