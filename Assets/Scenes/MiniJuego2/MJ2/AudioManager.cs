using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instancia;
    // esta variable permite que otros scripts puedan llamar al AudioManager



    [Header("Audio Sources")]
    public AudioSource musicaSource;
    public AudioSource efectosSource;




    [Header("Música")]
    public AudioClip musicaFondo;



    [Header("Efectos")]
    public AudioClip sonidoMoneda;
    public AudioClip sonidoPuerta;
    public AudioClip sonidoCorrecto;
    public AudioClip sonidoIncorrecto;
    public AudioClip sonidoBoton;
    public AudioClip sonidoVictoria;
    public AudioClip sonidoLavadoraRoja;



    void Awake()
    {
        //si todavía no existe un AudioManager, este objeto se vuelve la instancia principal
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject); // para que el AudioManager no se destruya al cambiar de escena.
        }
        else
        {
            Destroy(gameObject);
        }
    }



    void Start()
    {
        ReproducirMusica();
    }



    public void ReproducirMusica()
    {
        if (musicaFondo != null && musicaSource != null)
        {
            musicaSource.clip = musicaFondo;
            musicaSource.loop = true;
            musicaSource.Play();
        }
    }



    public void ReproducirMoneda()
    {
        ReproducirEfecto(sonidoMoneda);
    }



    public void ReproducirPuerta()
    {
        ReproducirEfecto(sonidoPuerta);
    }



    public void ReproducirCorrecto()
    {
        ReproducirEfecto(sonidoCorrecto);
    }



    public void ReproducirIncorrecto()
    {
        ReproducirEfecto(sonidoIncorrecto);
    }



    public void ReproducirBoton()
    {
        ReproducirEfecto(sonidoBoton);
    }



    public void ReproducirVictoria()
    {
        ReproducirEfecto(sonidoVictoria);
    }



    public void ReproducirLavadoraRoja()
    {
        ReproducirEfecto(sonidoLavadoraRoja);
    }



    void ReproducirEfecto(AudioClip clip)
    {
        if (clip != null && efectosSource != null)
        {
            efectosSource.PlayOneShot(clip);
        }
    }
}