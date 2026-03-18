using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioLibrary", menuName = "Audio/AudioLibrary")]
public class AudioLibrary : ScriptableObject
{
    [Header("Music")]
    [SerializeField] private List<AudioEntry> musicTracks = new List<AudioEntry>();

    [Header("Sound Effects")]
    [SerializeField] private List<AudioEntry> sfxClips = new List<AudioEntry>();

    [Header("UI Sounds")]
    [SerializeField] private List<AudioEntry> uiClips = new List<AudioEntry>();

    [Header("Player SFX Collections")]
    [SerializeField] private List<AudioClip> playerFootstepSounds = new List<AudioClip>();

    [Header("Enemy SFX Collections")]
    //[SerializeField] private List<AudioClip> enemyHitSounds = new List<AudioClip>();

    private Dictionary<string, AudioClip> musicDict;
    private Dictionary<string, AudioClip> sfxDict;
    private Dictionary<string, AudioClip> uiDict;

    private void OnEnable()
    {
        BuildDictionaries();
    }

    private void BuildDictionaries()
    {
        musicDict = new Dictionary<string, AudioClip>();
        foreach (var entry in musicTracks)
        {
            if (!string.IsNullOrEmpty(entry.key) && entry.clip != null)
            {
                musicDict[entry.key] = entry.clip;
            }
        }

        sfxDict = new Dictionary<string, AudioClip>();
        foreach (var entry in sfxClips)
        {
            if (!string.IsNullOrEmpty(entry.key) && entry.clip != null)
            {
                sfxDict[entry.key] = entry.clip;
            }
        }

        uiDict = new Dictionary<string, AudioClip>();
        foreach (var entry in uiClips)
        {
            if (!string.IsNullOrEmpty(entry.key) && entry.clip != null)
            {
                uiDict[entry.key] = entry.clip;
            }
        }
    }

    public AudioClip GetMusic(string key)
    {
        if (musicDict == null) BuildDictionaries();
        return musicDict.TryGetValue(key, out AudioClip clip) ? clip : null;
    }

    public AudioClip GetSFX(string key)
    {
        if (sfxDict == null) BuildDictionaries();
        return sfxDict.TryGetValue(key, out AudioClip clip) ? clip : null;
    }

    public AudioClip GetUISFX(string key)
    {
        if (uiDict == null) BuildDictionaries();
        return uiDict.TryGetValue(key, out AudioClip clip) ? clip : null;
    }

    // RANDOM SFX Lists
    public AudioClip GetRandomPlayerFootstep() => GetRandom(playerFootstepSounds);

    private AudioClip GetRandom(List<AudioClip> list)
    {
        if (list == null || list.Count == 0) return null;
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    [Serializable]
    public class AudioEntry
    {
        public string key;
        public AudioClip clip;
    }
}
