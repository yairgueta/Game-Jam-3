using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Player;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Vector3 rot;
    private Vector3 back;
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            anim.SetBool("open", true);
        }
    }

    public void BackToIdle()
    {
        anim.SetBool("open", false);
    }


}
