using UnityEngine;
using Ximena.Sonido;

public class Obstaculo : MonoBehaviour
{
    [Header("Configuración del Obstáculo")]
    public TipoObstaculo tipo = TipoObstaculo.Fisico;

    public enum TipoObstaculo
    {
        Fisico, 
        Toxico 
    }

    [Header("Configuración de zona tóxica")]

    public float multiplicadorVelocidadToxico = 0.3f;

    public float duracionEfectoToxico = 2;

    [Header("Configuración de obstáculo físico")]

    public bool reducirVelocidadAlChocar = true;

    public float multiplicadorVelocidadFisico = 0.4f;
    public float duracionEfectoFisico = 1f;

    private VidasManager vidasManager;

    void Start()
    {
        vidasManager = FindObjectOfType<VidasManager>();

        if (vidasManager == null)
        {
            Debug.LogError("Obstaculo: No se encontró el VidasManager en la escena.");
        }

        ValidarConfiguracion();
    }

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
            Debug.LogWarning(
                gameObject.name +
                ": Es tipo Fisico pero isTrigger está activado."
            );
        }

        if (tipo == TipoObstaculo.Toxico && !col.isTrigger)
        {
            Debug.LogWarning(
                gameObject.name +
                ": Es tipo Toxico pero isTrigger está desactivado."
            );
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (tipo != TipoObstaculo.Fisico) return;


        if (collision.gameObject.CompareTag("Jugador"))
        {
           EscudoTemporal escudo =
    collision.gameObject.GetComponentInParent<EscudoTemporal>();

            if (escudo != null && escudo.escudoActivo)
            {
                Debug.Log(" Escudo bloqueó daño físico");

                SFXManager.instancia?.Mancha();

                return;
            }

            Debug.Log("¡Choque con obstáculo físico: " + gameObject.name + "!");


            vidasManager?.QuitarVida();


            SFXManager.instancia?.Mancha();

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
        if (tipo != TipoObstaculo.Toxico) return;

        if (otro.CompareTag("Jugador"))
        {
          EscudoTemporal escudo =
    otro.GetComponentInParent<EscudoTemporal>();

            if (escudo != null && escudo.escudoActivo)
            {
                Debug.Log("🛡️ Escudo protegió zona tóxica");

                SFXManager.instancia?.Mancha();

                return;
            }

            Debug.Log("¡Nave entró en zona tóxica: " + gameObject.name + "!");

            vidasManager?.QuitarVida();

            SFXManager.instancia?.Mancha();

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