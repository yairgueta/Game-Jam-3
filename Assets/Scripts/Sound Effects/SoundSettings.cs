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


    [Header("Events")] 
    public GameEvent onDayStart;
    public GameEvent onEclipseStart;
    public GameEvent onNightStart;
    public GameEvent onEnemyDeath;
    public GameEvent onPlayerDeath;
    public GameEvent onOutOfResources;
    public GameEvent onMushroomCollected;
    public GameEvent onBulletExplode;

    [Header("Attributes")] 
    public float volume;
    [Tooltip("The factor of change ease between two music clips")] public float mixerFactor;
}
