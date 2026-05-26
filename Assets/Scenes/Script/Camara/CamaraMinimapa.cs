using UnityEngine;

// Controla la cámara del minimapa
// Puede estar fija (para pistas pequeñas) o seguir al jugador
// Este script va en el GameObject "Camara_Minimapa"
public class CamaraMinimapa : MonoBehaviour
{
    [Header("Configuración")]
    // true = sigue al jugador / false = cámara fija sobre toda la pista
    public bool seguirJugador = false;

    // Referencia al jugador — se busca automáticamente si está vacío
    public Transform jugador;

    // Posición Z fija de la cámara
    private float posicionZ;

    void Start()
    {
        // Guardamos la Z original
        posicionZ = transform.position.z;

        // Si no asignaron el jugador, lo buscamos por tag
        if (jugador == null)
        {
            GameObject naveObj = GameObject.FindWithTag("Jugador");
            if (naveObj != null)
            {
                jugador = naveObj.transform;
                Debug.Log("CamaraMinimapa: Jugador encontrado → " + naveObj.name);
            }
            else
            {
                Debug.LogError("CamaraMinimapa: No se encontró objeto con tag 'Jugador'. " +
                            "Verifica que la nave tiene ese tag.");
            }
        }

        // Verificamos el Render Texture
        Camera cam = GetComponent<Camera>();
        if (cam.targetTexture == null)
        {
            Debug.LogError("CamaraMinimapa: No tiene Render Texture asignado. " +
                        "Arrastra RT_Minimapa al campo Target Texture.");
        }
        else
        {
            Debug.Log("CamaraMinimapa: Render Texture asignado → " +
                    cam.targetTexture.name);
        }

        Debug.Log("CamaraMinimapa: lista. Modo: " +
                (seguirJugador ? "siguiendo jugador" : "fija"));
    }

    void LateUpdate()
    {
        // Solo seguimos si está configurado así y hay jugador
        if (!seguirJugador || jugador == null) return;

        // Seguimos X e Y del jugador pero mantenemos Z fija
        transform.position = new Vector3(
            jugador.position.x,
            jugador.position.y,
            posicionZ
        );
    }
}