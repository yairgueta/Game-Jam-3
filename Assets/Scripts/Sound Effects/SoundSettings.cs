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

    [Header("Attributes")] 
    public float bgVolume;
    public float sfxVolume;
    public float fadeoutTime;
    public float fadeInTime;
}
