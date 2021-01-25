using Cycles;
using DG.Tweening;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private SoundSettings soundSettings;
    [SerializeField] private AudioSource soundEffectsSource;
    [SerializeField] private AudioSource dayMusicSource;
    [SerializeField] private AudioSource nightMusicSource;
    [SerializeField] private AudioSource eclipseMusicSource;

    private AudioSource currentBGMusic;
    private AudioSource changeTo;
    private bool shouldChangeBG;
    private bool bgMusicChanged;
    
    // Start is called before the first frame update
    void Start()
    {
        InitializeBGClips();
        CyclesManager.Instance.DaySettings.OnCycleStart.Register(gameObject, o => PlayBGMusic(dayMusicSource));
        // soundSettings.onDayStart.Register(gameObject, o => PlayBGMusic(dayMusicSource));
        soundSettings.onEclipseStart.Register(gameObject,o => PlayBGMusic(eclipseMusicSource));
        soundSettings.onNightStart.Register(gameObject,o => PlayBGMusic(nightMusicSource));
        soundSettings.onEnemyDeath.Register(gameObject, o => PlaySoundEffect(soundSettings.enemyDeath));
        // soundSettings.onPlayerDeath.Register(gameObject, o => PlaySoundEffect(soundSettings.playerDeath));
        // soundSettings.onOutOfResources.Register(gameObject, o => PlaySoundEffect(soundSettings.outOfResources));
        // soundSettings.onMushroomCollected.Register(gameObject, o => PlaySoundEffect(soundSettings.mushroomCollected));
        // soundSettings.onBulletExplode.Register(gameObject, o => PlaySoundEffect(soundSettings.bulletExploded));
    }

    private void InitializeBGClips()
    {
        dayMusicSource.clip = soundSettings.dayMusic;
        nightMusicSource.clip = soundSettings.nightMusic;
        eclipseMusicSource.clip = soundSettings.eclipseMusic;
    }

    private void StartMusic(AudioSource source)
    {
        Debug.Log("playing sound");
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

    public void PlaySoundEffect(AudioClip audioClip)
    {
        if (audioClip == null) return;
        soundEffectsSource.PlayOneShot(audioClip, soundSettings.sfxVolume);
    }
}
