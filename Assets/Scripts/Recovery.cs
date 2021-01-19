using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class Recovery : MonoBehaviour
{
    private bool recoveryMode;
    private bool isEclipseTime;
    private float timer;
    [SerializeField] private float recoveryGaps = 7f;
    [SerializeField] private float lifeAddingAmount = 5f;
    void Start()
    {
        recoveryMode = false;
        timer = 0f;
        
    }

    void Update()
    {
        if (!isEclipseTime) return;

        if (!recoveryMode) return;
        
        if (timer <= 0)
        {
            PlayerController.PlayerSettings.UpdateLife(lifeAddingAmount);
            Debug.Log("added life");
            timer = recoveryGaps;
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
            Debug.Log("triggered sheep"+other.name);
            recoveryMode = true;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (!isEclipseTime) return;
        if (!recoveryMode) return;
        if (other.CompareTag("Sheep"))
        {
            Debug.Log("out of recovery mode");
            recoveryMode = false;
        }
    }
}
