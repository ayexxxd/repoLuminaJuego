using UnityEngine;

// Teletransportador — cuando la nave entra, sale por el portal destino
// Mantiene la velocidad y dirección de la nave al teletransportar
// Cada portal necesita una referencia al portal destino
public class Teletransportador : MonoBehaviour
{
    [Header("Configuración")]
    // Arrastra aquí el portal destino (el otro portal de la pareja)
    public Transform portalDestino;

    // Tiempo de invencibilidad después de teletransportar
    // Evita que la nave entre de nuevo inmediatamente
    public float tiempoInvencibilidad = 1.5f;

    [Header("Efecto visual")]
    public Color colorPortal = new Color(0.6f, 0f, 1f, 0.8f);
    public float velocidadPulso = 2f;

    // ---- Variables privadas ----
    private bool puedeTeletransportar = true;
    private SpriteRenderer spriteRenderer;
    private Vector3 escalaOriginal;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        escalaOriginal = transform.localScale;

        if (spriteRenderer != null)
            spriteRenderer.color = colorPortal;

        // Verificamos que tiene destino
        if (portalDestino == null)
        {
            Debug.LogError(gameObject.name + ": No tiene portal destino asignado. " +
                          "Arrastra el otro portal al campo Portal Destino.");
        }
    }

    void Update()
    {
        // Efecto de pulso visual
        float pulso = Mathf.PingPong(Time.time * velocidadPulso, 1f);
        transform.localScale = escalaOriginal * (1f + pulso * 0.15f);
    }

    void OnTriggerEnter2D(Collider2D otro)
    {
        // Solo el jugador y solo si puede teletransportar
        if (!otro.CompareTag("Jugador") || !puedeTeletransportar) return;
        if (portalDestino == null) return;

        Debug.Log(" ¡Teletransportando jugador desde " + gameObject.name +
                  " hacia " + portalDestino.name + "!");

        // Guardamos la velocidad actual de la nave
        Rigidbody2D rb = otro.GetComponent<Rigidbody2D>();
        Vector2 velocidadActual = Vector2.zero;
        if (rb != null) velocidadActual = rb.linearVelocity;

        // Movemos la nave al portal destino
        otro.transform.position = portalDestino.position;

        // Restauramos la velocidad (la nave no se detiene al teletransportar)
        if (rb != null) rb.linearVelocity = velocidadActual;

        // Mostramos mensaje en pantalla
        UIManager ui = FindObjectOfType<UIManager>();
        ui?.MostrarMensajeTemporal(" ¡Teletransporte!", 1.5f);

        // Activamos invencibilidad temporal en ambos portales
        // para evitar teletransporte inmediato de vuelta
        StartCoroutine(CorrutinaInvencibilidad());

        // También bloqueamos el portal destino temporalmente
        Teletransportador destinoScript =
            portalDestino.GetComponent<Teletransportador>();
        if (destinoScript != null)
            StartCoroutine(destinoScript.CorrutinaInvencibilidad());
    }

    // ---- Corrutina de invencibilidad temporal ----
    public System.Collections.IEnumerator CorrutinaInvencibilidad()
    {
        puedeTeletransportar = false;

        // Efecto visual — portal se vuelve semitransparente
        if (spriteRenderer != null)
            spriteRenderer.color = new Color(colorPortal.r, colorPortal.g,
                                            colorPortal.b, 0.3f);

        yield return new WaitForSeconds(tiempoInvencibilidad);

        // Restauramos el portal
        if (spriteRenderer != null)
            spriteRenderer.color = colorPortal;

        puedeTeletransportar = true;
    }
}