using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    [Header("Settings")]
    [Range(0f, 1f)] public float volume = 0.5f;

    [Header("Audio Clips")]
    public List<AudioClip> clips = new List<AudioClip>();

    [Header("Música de Resultado")]
    public AudioClip musicaVictoria;
    public AudioClip musicaDerrota;

    private AudioSource audioSource;
    private Dictionary<string, AudioClip> clipDictionary = new Dictionary<string, AudioClip>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // ← suscribirse
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        foreach (AudioClip clip in clips)
        {
            if (clip != null && !clipDictionary.ContainsKey(clip.name))
                clipDictionary.Add(clip.name, clip);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // ← limpiar suscripción
    }

    // ← Aquí defines qué música suena en cada escena
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "Menu":
            case "ExInInicio":
                PlayMusic("menu");
                break;
            case "ExInGameScene":
                PlayMusic("gameplay");
                break;
            case "ExInEndScene":
                bool victoria = PlayerPrefs.GetInt("Lives", 0) > 0;
                PlayMusic(victoria ? musicaVictoria : musicaDerrota);
                break;
            // Agrega más escenas según necesites
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        if (audioSource.clip == clip && audioSource.isPlaying) return;
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.volume = volume;
        audioSource.Play();
    }

    public void PlayMusic(string clipName)
    {
        if (clipDictionary.TryGetValue(clipName, out AudioClip clip))
        {
            // Evita reiniciar si ya está sonando el mismo clip
            if (audioSource.clip == clip && audioSource.isPlaying) return;

            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.volume = volume;
            audioSource.Play();
        }
        else
            Debug.LogWarning($"SFXManager: No se encontró el clip '{clipName}'.");
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void PlaySFX(AudioClip clip, bool randomizePitch = false, float volumeOverride = -1f)
    {
        if (clip == null) return;

        GameObject sfxObject = new GameObject("SFX_" + clip.name);
        AudioSource source = sfxObject.AddComponent<AudioSource>();

        source.clip = clip;
        source.volume = volumeOverride >= 0f ? volumeOverride : volume;

        if (randomizePitch)
            source.pitch = Random.Range(0.85f, 1.15f);

        source.Play();
        Destroy(sfxObject, clip.length);
    }

    public void Play(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip, volume);
        else
            Debug.LogWarning("SFXManager: AudioClip es null.");
    }
}