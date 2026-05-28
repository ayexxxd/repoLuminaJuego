using UnityEngine;

// Script que va en cada obstáculo de la pista
public class Obstaculo : MonoBehaviour
{
    [Header("Configuración del Obstáculo")] // Movi el Header aquí, encima de una variable
    public TipoObstaculo tipo = TipoObstaculo.Fisico;

    public enum TipoObstaculo
    {
        Fisico,  // Choque físico — rebota y quita vida
        Toxico   // Zona tóxica — se atraviesa, quita vida y reduce velocidad
    }
    [Header("Configuración de zona tóxica")]
    // Solo aplica si el tipo es Toxico
    // Qué tanto reduce la velocidad (0.3 = 30% de velocidad normal)
    public float multiplicadorVelocidadToxico = 0.3f;

    // Cuántos segundos dura el efecto de velocidad reducida
    public float duracionEfectoToxico = 2f;

    [Header("Configuración de obstáculo físico")]
    // Si el obstáculo físico también reduce velocidad al chocar
    public bool reducirVelocidadAlChocar = true;
    public float multiplicadorVelocidadFisico = 0.4f;
    public float duracionEfectoFisico = 1f;

    // ---- Referencias ----
    private VidasManager vidasManager;

    void Start()
    {
        vidasManager = FindObjectOfType<VidasManager>();

        if (vidasManager == null)
        {
            Debug.LogError("Obstaculo: No se encontró el VidasManager en la escena.");
        }

        // Verificamos que el collider coincide con el tipo de obstáculo
        ValidarConfiguracion();
    }

    // ---- Verifica que el collider está configurado correctamente ----
    void ValidarConfiguracion()
    {
        Collider2D col = GetComponent<Collider2D>();

        if (col == null)
        {
            Debug.LogError(gameObject.name + ": No tiene Collider2D.");
            return;
        }

        if (tipo == TipoObstaculo.Fisico && col.isTrigger)
        {
            Debug.LogWarning(gameObject.name + ": Es tipo Fisico pero isTrigger está activado. " +
                           "Desactiva isTrigger para que haya rebote físico.");
        }

        if (tipo == TipoObstaculo.Toxico && !col.isTrigger)
        {
            Debug.LogWarning(gameObject.name + ": Es tipo Toxico pero isTrigger está desactivado. " +
                           "Activa isTrigger para que la nave pueda atravesarlo.");
        }
    }

    // ---- Para obstáculos FÍSICOS (isTrigger = false) ----
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (tipo != TipoObstaculo.Fisico) return;

        if (collision.gameObject.CompareTag("Jugador"))
        {
            Debug.Log("¡Choque con obstáculo físico: " + gameObject.name + "!");

            // Quitamos una vida
            vidasManager?.QuitarVida();

            // Opcionalmente reducimos la velocidad
            if (reducirVelocidadAlChocar)
            {
                MovimientoNave nave = collision.gameObject.GetComponent<MovimientoNave>();
                if (nave != null)
                {
                    nave.AplicarEfectoVelocidad(multiplicadorVelocidadFisico, duracionEfectoFisico);
                }
            }
        }
    }

    // ---- Para zonas TÓXICAS (isTrigger = true) ----
    void OnTriggerEnter2D(Collider2D otro)
    {
        if (tipo != TipoObstaculo.Toxico) return;

        if (otro.CompareTag("Jugador"))
        {
            Debug.Log("¡Nave entró en zona tóxica: " + gameObject.name + "!");

            // Quitamos una vida
            vidasManager?.QuitarVida();

            // Reducimos la velocidad de la nave
            MovimientoNave nave = otro.GetComponent<MovimientoNave>();
            if (nave != null)
            {
                nave.AplicarEfectoVelocidad(multiplicadorVelocidadToxico, duracionEfectoToxico);
            }
        }
    }
}