using System.Collections;
using System.Collections.Generic;
using Cycles;
using Events;
using Player;
using UnityEngine;
using DG.Tweening;

public class ShearPopUp : MonoBehaviour
{
    [SerializeField] private SheepSettings sheepSettings;
    [SerializeField] private GameEvent onShear;
    [SerializeField] private GameEvent onSheepDeath;
    [SerializeField] private float duration;
    [SerializeField] private Ease ease;
    private Transform popUpTransform;
    private Vector3 originalScale; 
    private SpriteRenderer spriteRenderer;
    private bool notYetSheared;
    private Tween tween; 
    
    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        popUpTransform = GetComponentInChildren<Transform>();
        originalScale = popUpTransform.localScale;
        notYetSheared = true;
        spriteRenderer.enabled = false;
        onShear.Register(gameObject, PopOff);
        CyclesManager.Instance.EclipseSettings.OnCycleStart.Register(gameObject, PopUpShearMe);
        CyclesManager.Instance.EclipseSettings.OnCycleEnd.Register(gameObject, TakePopUpDown);
    }
    

    private void PopUpShearMe(object o)
    {
        if (!notYetSheared) return;
        spriteRenderer.enabled = true;
        transform.position = sheepSettings.sheeps[0].transform.position;
        float pos = transform.position.y;
        tween = DOTween.Sequence()
            .Append(transform.DOMoveY(pos*1.2f, duration).SetEase(ease))
            .Append(transform.DOMoveY(pos, duration).SetEase(ease))
            .SetLoops(-1);
    }

    private void TakePopUpDown(object o)
    {
        spriteRenderer.enabled = false;
        tween.Kill();
    }


    private void PopOff(object o)
    {
        notYetSheared = false;
        spriteRenderer.enabled = false;
        tween.Kill();
    }
    
    


}
