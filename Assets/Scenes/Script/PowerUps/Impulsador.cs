using UnityEngine;

public class Impulsador : MonoBehaviour
{
    [Header("Configuración del boost")]
    public float multiplicadorVelocidad = 2f;
    public float duracionBoost = 3f;

    [Header("Sistema Spawner")]
    public Transform[] puntosSpawn;

    public float tiempoReaparicion = 5f;

    [Header("Efecto visual")]
    public float velocidadRotacion = 60f;
    public Color colorNormal = new Color(0.2f, 0.8f, 1f, 0.8f);
    public Color colorActivo = new Color(1f, 0.9f, 0.2f, 1f);

    private SpriteRenderer spriteRenderer;
    private Collider2D miCollider;
    private bool estaActivo = true;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        miCollider     = GetComponent<Collider2D>();

        if (spriteRenderer != null)
            spriteRenderer.color = colorNormal;

        if (puntosSpawn == null || puntosSpawn.Length == 0)
        {
            Debug.LogWarning(gameObject.name + ": No hay puntos de spawn asignados. " +
                           "Arrastra los PuntosSpawn al array en el Inspector.");
        }
    }

    void Update()
    {
        if (estaActivo)
            transform.Rotate(0f, 0f, velocidadRotacion * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D otro)
    {
        if (!otro.CompareTag("Jugador") || !estaActivo) return;

        Debug.Log(" Impulsador tocado por el jugador.");

        MovimientoNave nave = otro.GetComponent<MovimientoNave>();
        if (nave != null)
            nave.AplicarEfectoVelocidad(multiplicadorVelocidad, duracionBoost);

        UIManager ui = FindObjectOfType<UIManager>();
        ui?.MostrarMensajeTemporal(" ¡IMPULSADOR!", 1.5f);

        PuntosManager.instancia?.AgregarPuntosPorEstrella();

        StartCoroutine(CorrutinaSpawner());
    }

    System.Collections.IEnumerator CorrutinaSpawner()
    {
        estaActivo = false;

        if (spriteRenderer != null) spriteRenderer.enabled = false;
        if (miCollider != null)     miCollider.enabled = false;

        Debug.Log("Impulsador desaparecido. Reaparecerá en " +
                  tiempoReaparicion + "s en posición aleatoria.");

        yield return new WaitForSeconds(tiempoReaparicion);

        if (puntosSpawn != null && puntosSpawn.Length > 0)
        {
            int indiceAleatorio = Random.Range(0, puntosSpawn.Length);
            transform.position = puntosSpawn[indiceAleatorio].position;

            Debug.Log("Impulsador reapareció en: " +
                      puntosSpawn[indiceAleatorio].name);
        }

        if (spriteRenderer != null) spriteRenderer.enabled = true;
        if (miCollider != null)     miCollider.enabled = true;

        estaActivo = true;
    }
}