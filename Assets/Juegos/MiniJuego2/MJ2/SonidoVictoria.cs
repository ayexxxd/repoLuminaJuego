using UnityEngine;

public class SonidoVictoria : MonoBehaviour
{
    void Start()
    {
        if (AudioManager.instancia != null)
        {
            AudioManager.instancia.ReproducirVictoria();
        }
    }
}