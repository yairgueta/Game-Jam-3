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
        soundSettings.onDayStart.Register(gameObject, o => PlayBGMusic(dayMusicSource));
        soundSettings.onEclipseStart.Register(gameObject,o => PlayBGMusic(eclipseMusicSource));
        soundSettings.onNightStart.Register(gameObject,o => PlayBGMusic(nightMusicSource));
        soundSettings.onEnemyDeath.Register(gameObject, o => PlaySoundEffect(soundSettings.enemyDeath));
    }

    private void Update()
    {
        if (!shouldChangeBG) return;
        DecreaseOldBGMusic();
        IncreaseNewBMusic();
    }

    private void DecreaseOldBGMusic()
    {
        if (bgMusicChanged) return;
        var oldBGnewVolume = currentBGMusic.volume - soundSettings.mixerFactor * 2f * Time.deltaTime;
        if (oldBGnewVolume <= 0)
        {
            currentBGMusic.Stop();
            currentBGMusic = changeTo;
            bgMusicChanged = true;
            return;
        }
        currentBGMusic.volume = oldBGnewVolume;
    }

    private void IncreaseNewBMusic()
    {
        var newBGVolume = changeTo.volume + soundSettings.mixerFactor * Time.deltaTime;
        if (newBGVolume >= soundSettings.volume)
        {
            changeTo.volume = soundSettings.volume;
            shouldChangeBG = false;
            return;
        }
        changeTo.volume = newBGVolume;
    }
    
    private void StartMusic(AudioSource source)
    {
        currentBGMusic = source;
        currentBGMusic.volume = soundSettings.volume;
        currentBGMusic.Play();
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
        changeTo = source;
        changeTo.volume = 0f;
        changeTo.Play();
        shouldChangeBG = true;
        bgMusicChanged = false;
    }

    public void PlaySoundEffect(AudioClip audioClip)
    {
        if (!audioClip) return;
        soundEffectsSource.PlayOneShot(audioClip);
    }
}
