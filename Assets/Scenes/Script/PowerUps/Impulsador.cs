using UnityEngine;

// Script que va en cada impulsador de la pista
// Cuando la nave pasa sobre él, recibe un boost de velocidad temporal
// Requiere que el Collider2D tenga isTrigger = true
public class Impulsador : MonoBehaviour
{
    [Header("Configuración del boost")]
    // Cuánto multiplica la velocidad durante el boost
    // 2 = el doble de velocidad normal
    public float multiplicadorVelocidad = 2f;

    // Cuántos segundos dura el boost
    public float duracionBoost = 3f;

    [Header("Efecto visual del impulsador")]
    // Velocidad a la que rota el sprite del impulsador
    public float velocidadRotacion = 60f;

    // Color normal del impulsador
    public Color colorNormal = new Color(0.2f, 0.8f, 1f, 0.8f);

    // Color cuando la nave está sobre él
    public Color colorActivo = new Color(1f, 0.9f, 0.2f, 1f);

    // ---- Variables privadas ----
    private SpriteRenderer spriteRenderer;

    // Para evitar que se active varias veces seguidas
    private bool estaActivo = false;

    // Tiempo de espera entre activaciones
    public float tiempoRecarga = 1f;

    void Start()
    {
        // Buscamos el SpriteRenderer para cambiar colores
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Aplicamos el color normal al inicio
        if (spriteRenderer != null)
            spriteRenderer.color = colorNormal;

        // Verificamos que tiene isTrigger activado
        Collider2D col = GetComponent<Collider2D>();
        if (col != null && !col.isTrigger)
        {
            Debug.LogWarning(gameObject.name + ": El Collider2D no tiene isTrigger activado. " +
                        "El boost no funcionará. Actívalo en el Inspector.");
        }
    }

    void Update()
    {
        // Rotamos el sprite continuamente para que se vea animado
        transform.Rotate(0f, 0f, velocidadRotacion * Time.deltaTime);
    }

    // ---- Se llama cuando la nave ENTRA al área del impulsador ----
    void OnTriggerEnter2D(Collider2D otro)
    {
        // Solo reaccionamos si es el jugador y el impulsador no está en recarga
        if (!otro.CompareTag("Jugador") || estaActivo) return;

        Debug.Log("¡Impulsador activado por " + gameObject.name + "!");

        // Cambiamos el color para dar feedback visual inmediato
        if (spriteRenderer != null)
            spriteRenderer.color = colorActivo;

        // Buscamos el script de movimiento en la nave
        MovimientoNave nave = otro.GetComponent<MovimientoNave>();

        if (nave != null)
        {
            // Aplicamos el boost de velocidad
            nave.AplicarEfectoVelocidad(multiplicadorVelocidad, duracionBoost);
            Debug.Log("Boost aplicado: x" + multiplicadorVelocidad +
                    " durante " + duracionBoost + "s");
        }
        else
        {
            Debug.LogError("Impulsador: No se encontró MovimientoNave en la nave. " +
                        "Verifica que la nave tiene el tag 'Jugador' y el script correcto.");
        }

        // Mostramos mensaje en pantalla si existe el UIManager
        UIManager ui = FindObjectOfType<UIManager>();
        if (ui != null)
            ui.MostrarMensajeTemporal("⚡ ¡BOOST!", 1.5f);

        // Sumamos puntos si existe el PuntosManager
        if (PuntosManager.instancia != null)
            PuntosManager.instancia.AgregarPuntosPorEstrella();

        // Iniciamos la recarga del impulsador
        StartCoroutine(CorrutinaRecarga());
    }

    // ---- Se llama cuando la nave SALE del área del impulsador ----
    void OnTriggerExit2D(Collider2D otro)
    {
        if (!otro.CompareTag("Jugador")) return;

        // Restauramos el color normal al salir
        if (spriteRenderer != null && !estaActivo)
            spriteRenderer.color = colorNormal;
    }

    // ---- Corrutina que maneja el tiempo de recarga ----
    // Durante la recarga el impulsador no puede activarse de nuevo
    System.Collections.IEnumerator CorrutinaRecarga()
    {
        estaActivo = true;

        // Esperamos el tiempo de recarga
        yield return new WaitForSeconds(tiempoRecarga);

        // Restauramos el color y permitimos nueva activación
        if (spriteRenderer != null)
            spriteRenderer.color = colorNormal;

        estaActivo = false;
        Debug.Log(gameObject.name + ": Impulsador listo de nuevo.");
    }
}