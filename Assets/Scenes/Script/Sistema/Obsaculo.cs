using UnityEngine;
using Ximena.Sonido;

// Script que va en cada obstáculo de la pista
public class Obstaculo : MonoBehaviour
{
    [Header("Configuración del Obstáculo")]
    public TipoObstaculo tipo = TipoObstaculo.Fisico;

    public enum TipoObstaculo
    {
        Fisico,  // Choque físico — rebota y quita vida
        Toxico   // Zona tóxica — se atraviesa, quita vida y reduce velocidad
    }

    [Header("Configuración de zona tóxica")]

    // Qué tanto reduce la velocidad
    public float multiplicadorVelocidadToxico = 0.3f;

    // Cuánto dura el efecto
    public float duracionEfectoToxico = 2f;

    [Header("Configuración de obstáculo físico")]

    // Si el obstáculo físico también reduce velocidad
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

        // Verificamos configuración
        ValidarConfiguracion();
    }

    // ---- Verifica collider y trigger ----
    void ValidarConfiguracion()
    {
        Collider2D col = GetComponent<Collider2D>();

        if (col == null)
        {
            Debug.LogError(gameObject.name + ": No tiene Collider2D.");
            return;
        }

        // Obstáculo físico NO debe ser trigger
        if (tipo == TipoObstaculo.Fisico && col.isTrigger)
        {
            Debug.LogWarning(
                gameObject.name +
                ": Es tipo Fisico pero isTrigger está activado."
            );
        }

        // Obstáculo tóxico SÍ debe ser trigger
        if (tipo == TipoObstaculo.Toxico && !col.isTrigger)
        {
            Debug.LogWarning(
                gameObject.name +
                ": Es tipo Toxico pero isTrigger está desactivado."
            );
        }
    }

    // =========================================================
    // OBSTÁCULOS FÍSICOS
    // =========================================================
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Solo para obstáculos físicos
        if (tipo != TipoObstaculo.Fisico) return;

        // Verificamos que sea el jugador
        if (collision.gameObject.CompareTag("Jugador"))
        {
            // ---- REVISAMOS EL ESCUDO ----
           EscudoTemporal escudo =
    collision.gameObject.GetComponentInParent<EscudoTemporal>();

            // Si tiene escudo activo no recibe daño
            if (escudo != null && escudo.escudoActivo)
            {
                Debug.Log("🛡️ Escudo bloqueó daño físico");

                // Sonido opcional
                SFXManager.instancia?.Mancha();

                return;
            }

            Debug.Log("¡Choque con obstáculo físico: " + gameObject.name + "!");

            // Quitamos vida
            vidasManager?.QuitarVida();

            // Sonido
            SFXManager.instancia?.Mancha();

            // Reducimos velocidad opcionalmente
            if (reducirVelocidadAlChocar)
            {
                MovimientoNave nave =
                    collision.gameObject.GetComponent<MovimientoNave>();

                if (nave != null)
                {
                    nave.AplicarEfectoVelocidad(
                        multiplicadorVelocidadFisico,
                        duracionEfectoFisico
                    );
                }
            }
        }
    }


    void OnTriggerEnter2D(Collider2D otro)
    {
        // Solo para tóxicos
        if (tipo != TipoObstaculo.Toxico) return;

        // Verificamos jugador
        if (otro.CompareTag("Jugador"))
        {
            // ---- REVISAMOS EL ESCUDO ----
          EscudoTemporal escudo =
    otro.GetComponentInParent<EscudoTemporal>();

            // Si tiene escudo activo ignoramos daño
            if (escudo != null && escudo.escudoActivo)
            {
                Debug.Log("🛡️ Escudo protegió zona tóxica");

                // Sonido opcional
                SFXManager.instancia?.Mancha();

                return;
            }

            Debug.Log("¡Nave entró en zona tóxica: " + gameObject.name + "!");

            // Quitamos vida
            vidasManager?.QuitarVida();

            // Sonido
            SFXManager.instancia?.Mancha();

            // Reducimos velocidad
            MovimientoNave nave =
                otro.GetComponent<MovimientoNave>();

            if (nave != null)
            {
                nave.AplicarEfectoVelocidad(
                    multiplicadorVelocidadToxico,
                    duracionEfectoToxico
                );
            }
        }
    }
}