using UnityEngine;

// Script que va en las zonas de boost fijas de la pista
// Cuando la nave pasa por aquí, recibe un boost de velocidad
public class ZonaBoost : MonoBehaviour
{
    [Header("Efecto visual")]
    // Color que toma la zona cuando la nave está sobre ella
    public Color colorActivo = new Color(1f, 0.9f, 0.2f, 0.9f);
    public Color colorNormal = new Color(1f, 0.9f, 0.2f, 0.5f);

    // Velocidad de rotación del sprite del boost
    public float velocidadRotacion = 45f;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.color = colorNormal;
        }
    }

    void Update()
    {
        // Rotamos el sprite para que se vea animado
        transform.Rotate(0, 0, velocidadRotacion * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D otro)
    {
        if (!otro.CompareTag("Jugador")) return;

        Debug.Log("¡Nave entró a zona boost!");

        // Cambiamos el color para dar feedback visual
        if (spriteRenderer != null)
        {
            spriteRenderer.color = colorActivo;
        }

        // Aplicamos el boost a través del PowerUpManager
        if (PowerUpManager.instancia != null)
        {
            PowerUpManager.instancia.AplicarBoost();
        }
    }

    void OnTriggerExit2D(Collider2D otro)
    {
        if (!otro.CompareTag("Jugador")) return;

        // Restauramos el color al salir
        if (spriteRenderer != null)
        {
            spriteRenderer.color = colorNormal;
        }
    }
}