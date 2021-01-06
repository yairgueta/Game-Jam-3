using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesAnimation : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;
    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnMouseEnter()
    {
        animator.SetBool("shouldGlow", true);
        particles.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        animator.SetBool("shouldGlow", false);
        particles.gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        animator.SetBool("shouldGlow", false);
        particles.gameObject.SetActive(false);
    }
}
