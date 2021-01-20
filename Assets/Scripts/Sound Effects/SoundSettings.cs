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

    [Header("Attributes")] 
    public float volume;
    [Tooltip("The factor of change ease between two music clips")] public float mixerFactor;
}
