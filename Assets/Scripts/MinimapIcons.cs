using System.Collections;
using System.Collections.Generic;
using Cycles;
using UnityEngine;
using Object = System.Object;

public class MinimapIcons : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
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
