using System.Collections;
using System.Collections.Generic;
using Cycles;
using DG.Tweening;
using UnityEngine;
using Object = System.Object;

public class MinimapIcons : MonoBehaviour
{
    private Animator anim;
    private Tween tween;
    private SpriteRenderer sheepRenderer;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        CyclesManager.Instance.NightSettings.OnCycleEnd.Register(gameObject, DoneAttack);
        CyclesManager.Instance.NightSettings.OnCycleStart.Register(gameObject, StartAttack);
        CyclesManager.Instance.EclipseSettings.OnCycleStart.Register(gameObject, OnEclipse);
        CyclesManager.Instance.EclipseSettings.OnCycleEnd.Register(gameObject, DoneEclipse);

    }

    public void StartAttack(object o)
    {
        anim.SetBool("isAttacking", true);
        // tween = DOTween.Sequence()
            // .Append(DOTween.To(() => sheepRenderer.color.a, f => sheepRenderer.color = f, 0f, 1f))
            // .Append(DOTween.To(() => light2D.intensity, f => light2D.intensity = f, 1.24F, 0.2f).SetDelay(0.1f));

    }
    
    public void DoneAttack(object o)
    {
        anim.SetBool("isAttacking", false);
    }

    public void OnEclipse(object o)
    {
        anim.SetBool("isEclipse", true);
    }
    
    public void DoneEclipse(object o)
    {
        anim.SetBool("isEclipse", false);
    }
}
