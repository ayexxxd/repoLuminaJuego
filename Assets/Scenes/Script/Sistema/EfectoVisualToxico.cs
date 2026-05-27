using UnityEngine;

// Hace que un sprite pulse (agrande y achique) para indicar que es peligroso
// Úsalo en zonas tóxicas para que el jugador las identifique fácilmente
public class EfectoVisualToxico : MonoBehaviour
{
    [Header("Configuración del pulso")]
    // Qué tan grande se pone en el punto máximo del pulso
    // 1.1 = 10% más grande que su tamaño original
    public float escalMaxima = 1.1f;

    // Qué tan pequeño se pone en el punto mínimo
    public float escalaMinima = 0.9f;

    // Qué tan rápido pulsa
    public float velocidadPulso = 2f;

    // Color base del sprite
    public Color colorBase = new Color(0.5f, 0f, 0.5f, 0.7f); // Morado semitransparente

    // Color en el punto máximo del pulso
    public Color colorPulso = new Color(1f, 0f, 0.5f, 0.9f); // Rosa brillante

    // ---- Variables privadas ----
    private Vector3 escalaOriginal;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Guardamos la escala original para no perderla
        escalaOriginal = transform.localScale;

        spriteRenderer = GetComponent<SpriteRenderer>();

        // Aplicamos el color base
        if (spriteRenderer != null)
        {
            spriteRenderer.color = colorBase;
        }
    }

    void Update()
    {
        // Mathf.PingPong va de 0 a 1 y de 1 a 0 continuamente
        // Lo usamos para crear el efecto de pulso suave
        float pulso = Mathf.PingPong(Time.time * velocidadPulso, 1f);

        // Interpolamos entre escala mínima y máxima usando el pulso
        float escalaActual = Mathf.Lerp(escalaMinima, escalMaxima, pulso);
        transform.localScale = escalaOriginal * escalaActual;

        // También interpolamos el color para que brille
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.Lerp(colorBase, colorPulso, pulso);
        }
    }
}