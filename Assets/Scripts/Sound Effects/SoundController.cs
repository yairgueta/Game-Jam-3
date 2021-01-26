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

    // Start is called before the first frame update
    void Start()
    {
        InitializeSettings();
        InitializeAudioSources();
        InitializeBGClips();
        RegisterToEvents();
        // PlayAmbient();
        PlayMenuMusic();
    }

    private void InitializeSettings()
    {
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
        soundSettings.onPlayerDeath.Register(gameObject, o => PlaySoundEffect(soundSettings.playerDeath));
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

    private void StartMusic(AudioSource source)
    {
        currentBGMusic = source;
        changeTo = source;
        currentBGMusic.volume = 0f;
        currentBGMusic.Play();
        changeTo.DOFade(soundSettings.bgVolume, soundSettings.fadeInTime).SetEase(Ease.OutQuad);
    }

    private void PlayBGMusic(AudioSource source)
    {
        if (currentBGMusic == null)
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
        changeTo.DOFade(soundSettings.bgVolume, soundSettings.fadeInTime).SetEase(Ease.OutQuad);
    }

    private void PlayAmbient()
    {
        ambientMusicSource.volume = soundSettings.ambientVolume;
        ambientMusicSource.clip = soundSettings.ambient;
        ambientMusicSource.Play();
    }

    private void PlayMenuMusic()
    {
        mainMenuSource.volume = soundSettings.bgVolume;
        mainMenuSource.clip = soundSettings.eclipseMusic;
        mainMenuSource.Play();
    }

    public void TurnMenuMusicOff()
    {
        mainMenuSource.DOFade(0f, soundSettings.fadeoutTime).SetEase(Ease.OutQuad);
    }

    public void ChangeMenuVolume(float volume)
    {
        mainMenuSource.volume = volume;
    }

    public void PlaySoundEffect(AudioClip audioClip)
    {
        if (audioClip == null) return;
        soundEffectsSource.PlayOneShot(audioClip, soundSettings.sfxVolume);
    }
}
