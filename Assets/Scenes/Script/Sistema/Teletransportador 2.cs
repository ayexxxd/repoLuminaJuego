using UnityEngine;

// Teletransportador — portal que mueve la nave a otro punto de la pista
// Cada portal necesita una referencia al portal destino
// Requiere: Sprite Renderer, Circle Collider 2D con isTrigger = true
public class Teletransportador : MonoBehaviour
{
    [Header("Conexión de portales")]
    // Arrastra aquí el otro portal (el destino)
    // Portal_A apunta a Portal_B y viceversa
    public Transform portalDestino;

    [Header("Configuración")]
    // Segundos de gracia después de teletransportar
    // Durante este tiempo ningún portal puede activarse
    // Evita el bug de teletransporte infinito
    public float tiempoGracia = 1.5f;

    [Header("Efecto visual")]
    public Color colorNormal  = new Color(0.6f, 0f, 1f, 0.8f);
    public Color colorInactivo = new Color(0.3f, 0.3f, 0.3f, 0.5f);
    public float velocidadRotacion = 45f;
    public float velocidadPulso = 2f;

    // ---- Variables privadas ----
    private bool puedeTeletransportar = true;
    private SpriteRenderer spriteRenderer;
    private Vector3 escalaOriginal;

    // Variable estática compartida entre TODOS los portales
    // Cuando cualquier portal teletransporta, todos esperan
    // Esto es la clave para evitar el loop infinito
    private static float tiempoUltimoTeletransporte = -999f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        escalaOriginal = transform.localScale;

        // Aplicamos el color del portal
        if (spriteRenderer != null)
            spriteRenderer.color = colorNormal;

        // Verificaciones de configuración
        if (portalDestino == null)
        {
            Debug.LogError(gameObject.name + ": No tiene portal destino. " +
                          "Arrastra el otro portal al campo Portal Destino.");
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col == null)
        {
            Debug.LogError(gameObject.name + ": No tiene Collider2D.");
        }
        else if (!col.isTrigger)
        {
            Debug.LogError(gameObject.name + ": El Collider2D no tiene isTrigger = true.");
        }

        Debug.Log(gameObject.name + ": Portal listo. Destino: " +
                  (portalDestino != null ? portalDestino.name : "NO ASIGNADO"));
    }

    void Update()
    {
        // Efecto visual de pulso
        float pulso = Mathf.PingPong(Time.time * velocidadPulso, 1f);
        transform.localScale = escalaOriginal * (1f + pulso * 0.12f);

        // Rotamos el sprite
        transform.Rotate(0f, 0f, velocidadRotacion * Time.deltaTime);

        // Actualizamos el color según si puede teletransportar
        ActualizarColorSegunEstado();
    }

    void OnTriggerEnter2D(Collider2D otro)
    {
        // Solo reaccionamos si es el jugador
        if (!otro.CompareTag("Jugador")) return;

        // Verificamos el tiempo de gracia global
        // Si cualquier portal teletransportó recientemente esperamos
        float tiempoDesdeUltimo = Time.time - tiempoUltimoTeletransporte;
        if (tiempoDesdeUltimo < tiempoGracia)
        {
            Debug.Log(gameObject.name + ": En tiempo de gracia. " +
                      "Faltan " + (tiempoGracia - tiempoDesdeUltimo).ToString("F1") + "s");
            return;
        }

        // Verificamos que tenemos destino
        if (portalDestino == null)
        {
            Debug.LogError(gameObject.name + ": Portal destino no asignado.");
            return;
        }

        // ---- EJECUTAMOS EL TELETRANSPORTE ----
        EjecutarTeletransporte(otro.gameObject);
    }

    void EjecutarTeletransporte(GameObject jugador)
    {
        Debug.Log("Teletransportando desde " + gameObject.name +
                  " hacia " + portalDestino.name);

        // Guardamos el Rigidbody para conservar la velocidad
        Rigidbody2D rb = jugador.GetComponent<Rigidbody2D>();
        Vector2 velocidadActual = Vector2.zero;
        float velocidadAngular  = 0f;

        if (rb != null)
        {
            velocidadActual  = rb.linearVelocity;
            velocidadAngular = rb.angularVelocity;
        }

        // Movemos al jugador al portal destino
        // Usamos la posición del destino más un pequeño offset
        // para asegurarnos de que quede dentro del trigger del destino
        jugador.transform.position = portalDestino.position;

        // Conservamos la rotación de la nave
        // (la nave sigue apuntando en la misma dirección)
        // Si quieres que adopte la rotación del portal destino cambia esto:
        // jugador.transform.rotation = portalDestino.rotation;

        // Restauramos la velocidad exactamente como estaba
        if (rb != null)
        {
            rb.linearVelocity  = velocidadActual;
            rb.angularVelocity = velocidadAngular;
        }

        // Registramos el tiempo del teletransporte
        // Esto bloquea TODOS los portales por tiempoGracia segundos
        tiempoUltimoTeletransporte = Time.time;

        // Efecto de camera shake suave al teletransportar
        if (CameraShake.instancia != null)
            CameraShake.instancia.Shake(0.15f, 0.2f);

        // Mensaje en pantalla
        UIManager ui = FindObjectOfType<UIManager>();
        ui?.MostrarMensajeTemporal(" ¡Teletransporte!", 1.5f);

        Debug.Log("Teletransporte completado. Velocidad conservada: " + velocidadActual);
    }

    // ---- Actualiza el color según si el portal puede teletransportar ----
    void ActualizarColorSegunEstado()
    {
        if (spriteRenderer == null) return;

        float tiempoDesdeUltimo = Time.time - tiempoUltimoTeletransporte;
        bool enGracia = tiempoDesdeUltimo < tiempoGracia;

        // Color gris durante el tiempo de gracia, morado cuando está listo
        spriteRenderer.color = enGracia ? colorInactivo : colorNormal;
    }

    // ---- Reinicia el tiempo de gracia (útil para testing) ----
    [ContextMenu("Reiniciar tiempo de gracia")]
    public void ReiniciarGracia()
    {
        tiempoUltimoTeletransporte = -999f;
        Debug.Log("Tiempo de gracia reiniciado — portales listos.");
    }
}