using UnityEngine;
using Ximena.Sonido;

// Script que va en cada estrella de la pista
// Cuando la nave la toca aplica un efecto aleatorio y la hace desaparecer
// Requiere: SpriteRenderer, Collider2D con isTrigger=true, tag "Jugador" en la nave
public class Estrella : MonoBehaviour
{
    [Header("Configuración")]
    // Segundos hasta que reaparece (0 = no reaparece)
    public float tiempoReaparicion = 10f;

    // Si debe rotar visualmente
    public bool rotar = true;
    public float velocidadRotacion = 90f;

    // ---- Variables privadas ----
    private bool fueRecogida = false;
    private SpriteRenderer spriteRenderer;
    private Collider2D miCollider;
    public SFXManager sfx;
    void Start()
    {
        // Buscamos los componentes necesarios
        spriteRenderer = GetComponent<SpriteRenderer>();
        miCollider     = GetComponent<Collider2D>();

        // Verificaciones de configuración
        if (spriteRenderer == null)
        {
            Debug.LogError(gameObject.name + ": No tiene SpriteRenderer. " +
                        "Agrega uno con Add Component.");
            return;
        }

        if (spriteRenderer.sprite == null)
        {
            Debug.LogError(gameObject.name + ": El SpriteRenderer no tiene sprite asignado. " +
                        "Arrastra un sprite al campo Source Image.");
            return;
        }

        if (miCollider == null)
        {
            Debug.LogError(gameObject.name + ": No tiene Collider2D. " +
                        "Agrega un Circle Collider 2D.");
            return;
        }

        if (!miCollider.isTrigger)
        {
            Debug.LogError(gameObject.name + ": El Collider2D no tiene isTrigger activado.");
        }

        if (PowerUpManager.instancia == null)
        {
            Debug.LogWarning(gameObject.name + ": No se encontró el PowerUpManager. " +
                        "Verifica que existe en la escena.");
        }

        Debug.Log(gameObject.name + ": Estrella lista. Sprite: " +
                spriteRenderer.sprite.name);
    }

    void Update()
    {
        // Rotamos la estrella si no fue recogida
        if (rotar && !fueRecogida)
            transform.Rotate(0f, 0f, velocidadRotacion * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D otro)
    {
        // Log para ver qué entra al trigger
        Debug.Log(gameObject.name + ": algo entró al trigger → " +
                otro.gameObject.name + " (tag: " + otro.tag + ")");

        // Verificamos que no fue recogida ya y que es el jugador
        if (fueRecogida || !otro.CompareTag("Jugador")) return;

        Debug.Log(" ¡Estrella recogida: " + gameObject.name + "!");
        fueRecogida = true;

        // Aplicamos el efecto aleatorio
         if (SFXManager.instancia != null)
            SFXManager.instancia.Estrella();
        if (PowerUpManager.instancia != null)
        {
            PowerUpManager.instancia.AplicarEfectoAleatorio();
        }
        else
        {
            Debug.LogError(gameObject.name + ": PowerUpManager.instancia es null. " +
                        "Verifica que el GameObject PowerUpManager existe en la escena.");
        }

        // Iniciamos la desaparición y reaparición
        StartCoroutine(CorrutinaReaparicion());
    }

    System.Collections.IEnumerator CorrutinaReaparicion()
    {
        // Ocultamos la estrella
        if (spriteRenderer != null) spriteRenderer.enabled = false;
        if (miCollider != null)     miCollider.enabled = false;

        // Si no reaparece la desactivamos para siempre
        if (tiempoReaparicion <= 0f)
        {
            gameObject.SetActive(false);
            yield break;
        }

        // Esperamos el tiempo de reaparición
        yield return new WaitForSeconds(tiempoReaparicion);

        // Reaparecemos la estrella
        if (spriteRenderer != null) spriteRenderer.enabled = true;
        if (miCollider != null)     miCollider.enabled = true;

        fueRecogida = false;
        Debug.Log(gameObject.name + ": Estrella reapareció.");
    }
}