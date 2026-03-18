using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource uiSource;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Audio Libraries")]
    [SerializeField] private AudioLibrary audioLibrary;

    private const string MASTER_VOLUME = "MasterVolume";
    private const string MUSIC_VOLUME = "MusicVolume";
    private const string SFX_VOLUME = "SFXVolume";
    private const string UI_VOLUME = "UIVolume";

    private Coroutine musicFadeCoroutine;
    private const float DEFAULT_FADE_DURATION = 1f;

    private List<AudioSource> audioSourcePool = new List<AudioSource>();
    private const int INITIAL_POOL_SIZE = 5;

    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    private void Initialize()
    {
        for (int i = 0; i < INITIAL_POOL_SIZE; i++)
        {
            CreatePooledAudioSource();
        }

        LoadVolumeSettings();
    }

    private AudioSource CreatePooledAudioSource()
    {
        GameObject sourceObj = new GameObject("PooledAudioSource");
        sourceObj.transform.SetParent(transform);
        AudioSource source = sourceObj.AddComponent<AudioSource>();
        source.playOnAwake = false;
        audioSourcePool.Add(source);
        return source;
    }
    // MUSIC
    public void PlayMusic(string musicKey, bool loop = true, float fadeDuration = DEFAULT_FADE_DURATION)
    {
        AudioClip clip = audioLibrary.GetMusic(musicKey);
        PlayMusic(clip, loop, fadeDuration);
    }

    public void PlayMusic(AudioClip clip, bool loop = true, float fadeDuration = DEFAULT_FADE_DURATION)
    {
        if (musicSource == null || clip == null)
        {
            Debug.Log("Music source or clip is null");
            return;
        }

        if (musicSource.clip == clip && musicSource.isPlaying)
        {
            return;
        }

        if (fadeDuration > 0)
        {
            if (musicFadeCoroutine != null)
                StopCoroutine(musicFadeCoroutine);
            musicFadeCoroutine = StartCoroutine(CrossfadeMusic(clip, loop, fadeDuration));
        }
        else
        {
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.Play();
        }
    }

    public void StopMusic(float fadeDuration = DEFAULT_FADE_DURATION)
    {
        if (musicSource == null) return;

        if (fadeDuration > 0)
        {
            if (musicFadeCoroutine != null)
                StopCoroutine(musicFadeCoroutine);
            musicFadeCoroutine = StartCoroutine(FadeOutMusic(fadeDuration));
        }
        else
        {
            musicSource.Stop();
        }
    }

    public void PauseMusic()
    {
        if (musicSource != null)
            musicSource.Pause();
    }

    public void ResumeMusic()
    {
        if (musicSource != null)
            musicSource.UnPause();
    }

    private IEnumerator CrossfadeMusic(AudioClip newClip, bool loop, float duration)
    {
        float startVolume = musicSource.volume;

        if (musicSource.isPlaying)
        {
            float elapsed = 0f;
            while (elapsed < duration / 2f)
            {
                elapsed += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / (duration / 2f));
                yield return null;
            }
        }

        musicSource.clip = newClip;
        musicSource.loop = loop;
        musicSource.Play();

        float elapsed2 = 0f;
        while (elapsed2 < duration / 2f)
        {
            elapsed2 += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, startVolume, elapsed2 / (duration / 2f));
            yield return null;
        }

        musicSource.volume = startVolume;
        musicFadeCoroutine = null;
    }

    private IEnumerator FadeOutMusic(float duration)
    {
        float startVolume = musicSource.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = startVolume;
        musicFadeCoroutine = null;
    }
    // SFX
    public void PlaySFX(string sfxKey, float volumeScale = 1f)
    {
        AudioClip clip = audioLibrary.GetSFX(sfxKey);
        PlaySFX(clip, volumeScale);
    }

    public void PlaySFX(AudioClip clip, float volumeScale = 1f)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip, volumeScale);
        }
    }

    public void PlaySFXWithPitch(string sfxKey, float pitch, float volumeScale = 1f)
    {
        AudioClip clip = audioLibrary.GetSFX(sfxKey);
        PlaySFXWithPitch(clip, pitch, volumeScale);
    }

    public void PlaySFXWithPitch(AudioClip clip, float pitch, float volumeScale = 1f)
    {
        if (clip == null) return;

        AudioSource source = GetAvailableAudioSource();
        source.pitch = pitch;
        source.PlayOneShot(clip, volumeScale);
        StartCoroutine(ResetPitchAfterPlay(source, clip.length));
    }

    public void PlaySFXWithRandomPitch(string sfxKey, float minPitch = 0.9f, float maxPitch = 1.1f, float volumeScale = 1f)
    {
        float randomPitch = UnityEngine.Random.Range(minPitch, maxPitch);
        PlaySFXWithPitch(sfxKey, randomPitch, volumeScale);
    }

    public void PlaySFXWithRandomPitch(AudioClip clip, float minPitch = 0.9f, float maxPitch = 1.1f, float volumeScale = 1f)
    {
        float randomPitch = UnityEngine.Random.Range(minPitch, maxPitch);
        PlaySFXWithPitch(clip, randomPitch, volumeScale);
    }
    public void PlayRandomSFX(List<AudioClip> clips, float volumeScale = 1f)
    {
        if (clips == null || clips.Count == 0) return;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Count)];
        PlaySFX(randomClip, volumeScale);
    }

    public void PlayUISFX(string sfxKey, float volumeScale = 1f)
    {
        AudioClip clip = audioLibrary.GetUISFX(sfxKey);
        PlayUISFX(clip, volumeScale);
    }

    public void PlayUISFX(AudioClip clip, float volumeScale = 1f)
    {
        if (uiSource != null && clip != null)
        {
            uiSource.PlayOneShot(clip, volumeScale);
        }
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach (var source in audioSourcePool)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        return CreatePooledAudioSource();
    }

    private IEnumerator ResetPitchAfterPlay(AudioSource source, float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        source.pitch = 1f;
    }

    // VOLUME
    public void SetMasterVolume(float volume)
    {
        SetMixerVolume(MASTER_VOLUME, volume);
        PlayerPrefs.SetFloat(MASTER_VOLUME, volume);
    }

    public void SetMusicVolume(float volume)
    {
        SetMixerVolume(MUSIC_VOLUME, volume);
        PlayerPrefs.SetFloat(MUSIC_VOLUME, volume);
    }

    public void SetSFXVolume(float volume)
    {
        SetMixerVolume(SFX_VOLUME, volume);
        PlayerPrefs.SetFloat(SFX_VOLUME, volume);
    }

    public void SetUIVolume(float volume)
    {
        SetMixerVolume(UI_VOLUME, volume);
        PlayerPrefs.SetFloat(UI_VOLUME, volume);
    }
    
    //RANDOM SFX PLAY

    public void PlayRandomPlayerFootstep(float volumeScale = 1f, float minPitch = 0.95f, float maxPitch = 1.05f)
    {
        if (audioLibrary == null) return;

        AudioClip clip = audioLibrary.GetRandomPlayerFootstep();
        if (clip != null)
        {
            PlaySFXWithRandomPitch(clip, minPitch, maxPitch, volumeScale);
        }
        else
        {
            Debug.LogWarning("No player footstep sounds available in AudioLibrary");
        }
    }


    private void SetMixerVolume(string parameterName, float volume)
    {
        if (audioMixer == null) return;

        // convert 0-1 range to decibels (-80 to 0) or ur ears are cooked
        float db = volume > 0 ? Mathf.Log10(volume) * 20 : -80f;
        audioMixer.SetFloat(parameterName, db);
    }

    private void LoadVolumeSettings()
    {
        SetMasterVolume(PlayerPrefs.GetFloat(MASTER_VOLUME, 1f));
        SetMusicVolume(PlayerPrefs.GetFloat(MUSIC_VOLUME, 1f));
        SetSFXVolume(PlayerPrefs.GetFloat(SFX_VOLUME, 1f));
        SetUIVolume(PlayerPrefs.GetFloat(UI_VOLUME, 1f));
    }

    public float GetMasterVolume() => PlayerPrefs.GetFloat(MASTER_VOLUME, 1f);
    public float GetMusicVolume() => PlayerPrefs.GetFloat(MUSIC_VOLUME, 1f);
    public float GetSFXVolume() => PlayerPrefs.GetFloat(SFX_VOLUME, 1f);
    public float GetUIVolume() => PlayerPrefs.GetFloat(UI_VOLUME, 1f);

    public bool IsMusicPlaying() => musicSource != null && musicSource.isPlaying;

    public void StopAllSounds()
    {
        StopMusic(0f);
        if (sfxSource != null) sfxSource.Stop();
        if (uiSource != null) uiSource.Stop();

        foreach (var source in audioSourcePool)
        {
            source.Stop();
        }
    }
}
