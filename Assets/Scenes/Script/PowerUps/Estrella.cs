using UnityEngine;

public class Estrella : MonoBehaviour
{
    [Header("Configuración")]
    // Cuántos segundos tarda en reaparecer después de ser recogida
    // 0 = no reaparece nunca
    public float tiempoReaparicion = 10f;

    // Si la estrella debe rotar visualmente
    public bool rotar = true;
    public float velocidadRotacion = 90f;

    // ---- Variables privadas ----
    private bool fueRecogida = false;
    private SpriteRenderer spriteRenderer;
    private Collider2D miCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        miCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        // Rotamos la estrella visualmente si está activa
        if (rotar && !fueRecogida)
        {
            // Rotamos en el eje Z — en 2D es la única rotación visible
            transform.Rotate(0, 0, velocidadRotacion * Time.deltaTime);
        }
    }

    // ---- Se llama cuando la nave entra al trigger de la estrella ----
    void OnTriggerEnter2D(Collider2D otro)
    {
        // Solo reaccionamos si es el jugador y la estrella no fue recogida ya
        if (!otro.CompareTag("Jugador") || fueRecogida) return;

        // Marcamos como recogida para evitar doble activación
        fueRecogida = true;

        Debug.Log("¡Estrella recogida!");

        // Pedimos al PowerUpManager que aplique un efecto aleatorio
        if (PowerUpManager.instancia != null)
        {
            PowerUpManager.instancia.AplicarEfectoAleatorio();
        }
        else
        {
            Debug.LogError("Estrella: No se encontró el PowerUpManager.");
        }

        // Iniciamos la corrutina de desaparición y reaparición
        StartCoroutine(CorrutinaReaparicion());
    }

    // ---- Hace desaparecer la estrella y la hace reaparecer después ----
    System.Collections.IEnumerator CorrutinaReaparicion()
    {
        // Ocultamos la estrella visualmente
        if (spriteRenderer != null) spriteRenderer.enabled = false;

        // Desactivamos el collider para que no se pueda recoger mientras está oculta
        if (miCollider != null) miCollider.enabled = false;

        // Si tiempoReaparicion es 0, la estrella desaparece para siempre
        if (tiempoReaparicion <= 0)
        {
            gameObject.SetActive(false);
            yield break; // Terminamos la corrutina
        }

        // Esperamos el tiempo de reaparición
        yield return new WaitForSeconds(tiempoReaparicion);

        // Reaparecemos la estrella
        if (spriteRenderer != null) spriteRenderer.enabled = true;
        if (miCollider != null) miCollider.enabled = true;

        fueRecogida = false;
        Debug.Log("Estrella reapareció.");
    }
}
