using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Events;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float intensity = 8;
    [SerializeField] private float time = .15f;
    private CinemachineVirtualCamera mainCam;
    private float shakeTimer = 0f;
    [SerializeField] private GameEvent onWallDestroy;
    [SerializeField] private GameEvent onSheepDeath;
    
    
    void Start()
    {
        mainCam = GetComponent<CinemachineVirtualCamera>();
        onWallDestroy.Register(gameObject, arg0 => Shake());
        onSheepDeath.Register(gameObject, arg0 => Shake());
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                var perlin = mainCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                perlin.m_AmplitudeGain = 0f;
            }
        }
    }

    public void Shake()
    {
        var perlin = mainCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }

}
