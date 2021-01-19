using System;
using System.Collections;
using System.Collections.Generic;
using Cycles;
using Player;
using UnityEngine;

public class Recovery : MonoBehaviour
{
    private bool recoveryMode;
    private bool isEclipseTime;
    private float timer;
    
    
    void Start()
    {
        recoveryMode = false;
        timer = 0f;
        CyclesManager.Instance.EclipseSettings.OnCycleStart.Register(gameObject, arg0 => ChangeToEclipseTime());
        CyclesManager.Instance.EclipseSettings.OnCycleEnd.Register(gameObject, arg0 => OutOfEclipseTime());

    }

    void Update()
    {
        if (!isEclipseTime) return;

        if (!recoveryMode) return;
        
        if (timer <= 0)
        {
            PlayerController.PlayerSettings.UpdateLife(PlayerController.PlayerSettings.lifeAdditionAmount);
            timer = PlayerController.PlayerSettings.recoveryGap;
        }
        timer -= Time.deltaTime;
    }


    private void ChangeToEclipseTime()
    {
        isEclipseTime = true;
    }
    
    private void OutOfEclipseTime()
    {
        isEclipseTime = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isEclipseTime) return;
        if (other.CompareTag("Sheep") && !recoveryMode)
        {
            recoveryMode = true;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (!isEclipseTime) return;
        if (!recoveryMode) return;
        if (other.CompareTag("Sheep"))
        {
            recoveryMode = false;
        }
    }
}
