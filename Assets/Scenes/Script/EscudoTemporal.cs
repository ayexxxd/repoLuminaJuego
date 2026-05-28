using UnityEngine;
using System.Collections;

public class EscudoTemporal : MonoBehaviour
{
    [Header("Configuración")]
    public bool escudoActivo = false;

    public float duracionEscudo = 5f;

    [Header("Visual")]
    public GameObject escudoVisual;

    private Coroutine rutinaEscudo;

    void Start()
    {
        // Make sure shield starts OFF
        escudoActivo = false;

        if (escudoVisual != null)
        {
            escudoVisual.SetActive(false);
        }
    }

    // =====================================================
    // ACTIVAR ESCUDO
    // =====================================================
    public void ActivarEscudo()
    {
        Debug.Log(" Activando escudo...");

        // Restart timer if already active
        if (rutinaEscudo != null)
        {
            StopCoroutine(rutinaEscudo);
        }

        rutinaEscudo = StartCoroutine(EscudoCoroutine());
    }

    IEnumerator EscudoCoroutine()
    {
        escudoActivo = true;

        Debug.Log("Escudo activo");

        // Show visual
        if (escudoVisual != null)
        {
            escudoVisual.SetActive(true);
        }

        // Wait duration
        yield return new WaitForSeconds(duracionEscudo);

        // Disable shield
        escudoActivo = false;

        Debug.Log(" Escudo desactivado");

        // Hide visual
        if (escudoVisual != null)
        {
            escudoVisual.SetActive(false);
        }

        rutinaEscudo = null;
    }
}