using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicLooper : MonoBehaviour
{
    [SerializeField] private bool playOnStart = true;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
    }

    private void Start()
    {
        if (playOnStart && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
