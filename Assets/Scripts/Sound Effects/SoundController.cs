using System;
using System.Collections;
using Cycles;
using DG.Tweening;
using UnityEngine;

public class SoundController : Singleton<SoundController>
{
    public SoundSettings soundSettings;
    private AudioSource soundEffectsSource;
    private AudioSource dayMusicSource;
    private AudioSource nightMusicSource;
    private AudioSource eclipseMusicSource;
    private AudioSource ambientMusicSource;
    private AudioSource mainMenuSource;

    private AudioSource currentBGMusic;
    private AudioSource changeTo;
    private bool shouldChangeBG;
    private bool bgMusicChanged;

    protected override void Awake()
    {
        DontDestroyOnLoad(gameObject);
        InitializeSettings();
        InitializeAudioSources();
        InitializeBGClips();
        RegisterToEvents();
        // PlayAmbient();
        PlayMenuMusic();
        soundSettings.onVolumeChange += ()=> ChangeBGVolume();
        base.Awake();
        
    }
    private void InitializeSettings()
    {
        soundSettings.SetBGMVolume(soundSettings.initialVolumes);
        soundSettings.SetSFXVolume(soundSettings.initialVolumes);
        if (soundSettings != null) return;
        var soundSetts = AssetBundle.FindObjectsOfType<SoundSettings>();
        if (soundSetts.Length > 1)
            Debug.LogWarning(
                "Sound settings ambiguity: sound settings was provided and there are more then one in project!");
        if (soundSetts.Length == 0)
        {
            Debug.LogError("Sound settings Problem: No sound settings were found!");
            return;
        }
        soundSettings = soundSetts[0];
    }

    private void RegisterToEvents()
    {
        CyclesManager.Instance.DaySettings.OnCycleStart.Register(gameObject, o => PlayBGMusic(dayMusicSource));
        // soundSettings.onDayStart.Register(gameObject, o => PlayBGMusic(dayMusicSource));
        soundSettings.onEclipseStart.Register(gameObject, o => PlayBGMusic(eclipseMusicSource));
        soundSettings.onNightStart.Register(gameObject, o => PlayBGMusic(nightMusicSource));
        soundSettings.onEnemyDeath.Register(gameObject, o => PlaySoundEffect(soundSettings.enemyDeath));
        // soundSettings.onPlayerDeath.Register(gameObject, o => PlaySoundEffect(soundSettings.playerDeath));
        soundSettings.onOutOfResources.Register(gameObject, o => PlaySoundEffect(soundSettings.outOfResources));
        soundSettings.onMushroomCollected.Register(gameObject, o => PlaySoundEffect(soundSettings.mushroomCollected));
        soundSettings.onBulletExplode.Register(gameObject, o => PlaySoundEffect(soundSettings.bulletExploded));
        soundSettings.onSheepDeath.Register(gameObject, o => PlaySoundEffect(soundSettings.sheepDeath));
        soundSettings.onWallDestroyed.Register(gameObject, o => PlaySoundEffect(soundSettings.wallDestroyed));
    }

    private void InitializeAudioSources()
    {
        var bgMusic = transform.GetChild(1);
        soundEffectsSource = transform.GetChild(0).GetComponent<AudioSource>();
        dayMusicSource = bgMusic.transform.GetChild(0).GetComponent<AudioSource>();
        nightMusicSource = bgMusic.transform.GetChild(1).GetComponent<AudioSource>();
        eclipseMusicSource = bgMusic.transform.GetChild(2).GetComponent<AudioSource>();
        ambientMusicSource = bgMusic.transform.GetChild(3).GetComponent<AudioSource>();
        mainMenuSource = bgMusic.transform.GetChild(4).GetComponent<AudioSource>();
    }

    private void InitializeBGClips()
    {
        dayMusicSource.clip = soundSettings.dayMusic;
        nightMusicSource.clip = soundSettings.nightMusic;
        eclipseMusicSource.clip = soundSettings.eclipseMusic;
    }

    private void ChangeBGVolume()
    {
        mainMenuSource.volume = soundSettings.BGMVolume;
        if (changeTo != null)
        {
            changeTo.volume = soundSettings.BGMVolume;
        }
        else if (currentBGMusic != null)
        {
            currentBGMusic.volume = soundSettings.BGMVolume;
        }
    }

    private void StartMusic(AudioSource source)
    {
        currentBGMusic = source;
        changeTo = source;
        currentBGMusic.volume = 0f;
        currentBGMusic.Play();
        changeTo.DOFade(soundSettings.BGMVolume, soundSettings.fadeInTime).SetEase(Ease.OutQuad);
    }

    private void PlayBGMusic(AudioSource source)
    {
        if (currentBGMusic == null || !currentBGMusic.isPlaying)
        {
            StartMusic(source);
            return;
        }

        ChangeMusic(source);
    }

    private void ChangeMusic(AudioSource source)
    {
        if (source == currentBGMusic) return;
        currentBGMusic = changeTo;
        changeTo = source;
        changeTo.volume = 0f;
        changeTo.Play();
        currentBGMusic.DOFade(0f, soundSettings.fadeoutTime).SetEase(Ease.OutQuad);
        // StartCoroutine(StopMusicDelay(currentBGMusic.clip.length));
        changeTo.DOFade(soundSettings.BGMVolume, soundSettings.fadeInTime).SetEase(Ease.OutQuad);
    }

    private void PlayAmbient()
    {
        ambientMusicSource.volume = soundSettings.ambientVolume;
        ambientMusicSource.clip = soundSettings.ambient;
        ambientMusicSource.Play();
    }

    private void PlayMenuMusic()
    {
        mainMenuSource.volume = soundSettings.BGMVolume;
        mainMenuSource.clip = soundSettings.eclipseMusic;
        mainMenuSource.Play();
    }

    public void TurnMenuMusicOff()
    {
        mainMenuSource.volume = 0f;
        mainMenuSource.Pause();
        // mainMenuSource.DOFade(0f, soundSettings.fadeoutTime).SetEase(Ease.OutQuad);
        // StopMusicDelay(soundSettings.fadeoutTime);
    }

    public void PlaySoundEffect(AudioClip audioClip)
    {
        if (audioClip == null) return;
        soundEffectsSource.PlayOneShot(audioClip, soundSettings.sfxVolume);
    }

    public void StopMusic()
    {
        mainMenuSource.Play();
        dayMusicSource.Pause();
        nightMusicSource.Pause();
        eclipseMusicSource.Pause();
        ambientMusicSource.Pause();
    }

    private IEnumerator StopMusicDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        mainMenuSource.Pause();
    }
}
