using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneMusicManager : MonoBehaviour
{
    [System.Serializable]
    public class SceneMusicMapping
    {
        public string sceneName;

        [Header("Music Settings")]
        [Tooltip("Leave empty to keep current music playing")]
        public string musicKey;

        [Tooltip("If true, stops all music in this scene")]
        public bool muteMusic = false;

        [Range(0f, 5f)] public float fadeDuration = 1.5f;
    }

    [SerializeField] private List<SceneMusicMapping> sceneMusicMap = new List<SceneMusicMapping>();
    [SerializeField] private float defaultFadeDuration = 1.5f;

    private string currentMusicKey;
    private bool isMusicMuted = false;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        PlayMusicForCurrentScene();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    private void PlayMusicForCurrentScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        PlayMusicForScene(sceneName);
    }

    private void PlayMusicForScene(string sceneName)
    {
        SceneMusicMapping mapping = sceneMusicMap.Find(m => m.sceneName == sceneName);

        if (mapping != null)
        {
            if (mapping.muteMusic)
            {
                Debug.Log($"Muting music for scene '{sceneName}'");
                AudioManager.Instance.StopMusic(mapping.fadeDuration);
                currentMusicKey = null;
                isMusicMuted = true;
                return;
            }
            if (!string.IsNullOrEmpty(mapping.musicKey))
            {
                if (currentMusicKey != mapping.musicKey)
                {
                    currentMusicKey = mapping.musicKey;
                    isMusicMuted = false;
                    AudioManager.Instance.PlayMusic(mapping.musicKey, true, mapping.fadeDuration);
                    Debug.Log($"Playing music '{mapping.musicKey}' for scene '{sceneName}'");
                }
                else
                {
                    Debug.Log($"Music '{mapping.musicKey}' already playing, continuing...");
                }
            }
            else
            {
                Debug.Log($"No music key specified for scene '{sceneName}', keeping current music");
            }
        }
        else
        {
            Debug.LogWarning($"No music mapping found for scene: {sceneName}");
        }
    }

    public void PlayMusicForScene(string sceneName, string musicKey, float fadeDuration)
    {
        currentMusicKey = musicKey;
        isMusicMuted = false;
        AudioManager.Instance.PlayMusic(musicKey, true, fadeDuration);
    }
    
    public bool IsMusicMuted() => isMusicMuted;
}