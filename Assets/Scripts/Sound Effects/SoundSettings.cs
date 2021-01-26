using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Sound Settings")]
public class SoundSettings : ScriptableObject
{
    [Header("Background Music")]
    public AudioClip dayMusic;
    public AudioClip nightMusic;
    public AudioClip eclipseMusic;

    [Header("Sound Effects")] 
    public AudioClip enemyDeath;
    public AudioClip playerDeath;
    public AudioClip outOfResources;
    public AudioClip mushroomCollected;
    public AudioClip bulletExploded;
    public AudioClip sheepDeath;
    public AudioClip wallDestroyed;
    public AudioClip pickStone;
    public AudioClip pickWood;
    public AudioClip monsterAttack;
    public AudioClip gate;
    public AudioClip ambient;
    
    [Header("Events")] 
    public GameEvent onDayStart;
    public GameEvent onEclipseStart;
    public GameEvent onNightStart;
    public GameEvent onEnemyDeath;
    public GameEvent onPlayerDeath;
    public GameEvent onOutOfResources;
    public GameEvent onMushroomCollected;
    public GameEvent onBulletExplode;
    public GameEvent onSheepDeath;
    public GameEvent onWallDestroyed;
    public Action onVolumeChange;
    
    [Header("Attributes")] 
    [Range(0,1)] public float initialVolumes;
    [Range(0,1)] private float bgmVolume;
    [Range(0,1)] public float sfxVolume;
    public float ambientVolume;
    public float fadeoutTime;
    public float fadeInTime;

    public float BGMVolume => bgmVolume;
    public void SetBGMVolume(float v)
    {
        bgmVolume = v;
        onVolumeChange?.Invoke();
    }

    public void SetSFXVolume(float v) => sfxVolume = v;
}
