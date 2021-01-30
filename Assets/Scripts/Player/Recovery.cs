using System;
using Cycles;
using UnityEngine;

namespace Player
{
    public class Recovery : MonoBehaviour
    {
        private bool recoveryMode;
        private bool isEclipseTime;
        private float timer;
        private ParticleSystem recoveryParticles;
    
    
        void Start()
        {
            recoveryParticles = GetComponentInChildren<ParticleSystem>();
            recoveryParticles.Stop();
            recoveryMode = false;
            timer = 0f;
            CyclesManager.Instance.EclipseSettings.OnCycleStart.Register(gameObject, arg0 => ChangeToEclipseTime());
            CyclesManager.Instance.EclipseSettings.OnCycleEnd.Register(gameObject, arg0 => OutOfEclipseTime());
        }

        void Update()
        {
            if (!isEclipseTime|| !recoveryMode) return;
        
                PlayerController.PlayerSettings.UpdateLife(PlayerController.PlayerSettings.lifeAdditionAmount);
            if (PlayerController.PlayerSettings.maxHealth - PlayerController.PlayerSettings.curHealth <= Mathf.Epsilon)
            {
                StopRecovery();
            }
        }
        
        private void ChangeToEclipseTime()
        {
            isEclipseTime = true;
        }
    
        private void OutOfEclipseTime()
        {
            isEclipseTime = false;
            recoveryParticles.Stop();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!isEclipseTime || !other.CompareTag("Sheep") || recoveryMode) return;
            recoveryMode = true;
            recoveryParticles.Play();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!isEclipseTime || !recoveryMode || !other.CompareTag("Sheep")) return;
            StopRecovery();
        }

        private void StopRecovery()
        {
            recoveryMode = false;
            recoveryParticles.Stop();
        }
    }
}
