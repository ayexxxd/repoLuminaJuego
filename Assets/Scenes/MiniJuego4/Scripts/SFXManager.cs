using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioClip bird;
    public AudioClip win;
    public AudioClip lose;

    public void BirdSound()
    {
        AudioSource.PlayClipAtPoint(bird, Camera.main.transform.position, 0.5f);
    }

    public void WinSound()
    {
        AudioSource.PlayClipAtPoint(win, Camera.main.transform.position, 0.5f);
    }

    public void LoseSound()
    {
        AudioSource.PlayClipAtPoint(lose, Camera.main.transform.position, 0.5f);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
