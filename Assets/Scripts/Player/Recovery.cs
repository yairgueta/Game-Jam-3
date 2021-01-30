using System;
using Cycles;
using UnityEngine;

namespace Player
{
    public class Recovery : MonoBehaviour
    {
        private bool recoveryMode;
        private bool isEclipseTime;
        private ParticleSystem recoveryParticles;
    
    
        void Start()
        {
            recoveryParticles = GetComponentInChildren<ParticleSystem>();
            recoveryParticles.Stop();
            recoveryMode = false;
            CyclesManager.Instance.EclipseSettings.OnCycleStart.Register(gameObject, ChangeToEclipseTime);
            CyclesManager.Instance.EclipseSettings.OnCycleEnd.Register(gameObject, OutOfEclipseTime);
        }

        void Update()
        {
            if (!isEclipseTime|| !recoveryMode) return;
        
            PlayerController.PlayerSettings.UpdateLife(PlayerController.PlayerSettings.lifeAdditionSpeed * Time.deltaTime);
            if (PlayerController.PlayerSettings.maxHealth == PlayerController.PlayerSettings.curHealth)
            {
                StopRecovery();
            }
        }
        
        private void ChangeToEclipseTime(object o)
        {
            isEclipseTime = true;
        }
    
        private void OutOfEclipseTime(object o)
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
