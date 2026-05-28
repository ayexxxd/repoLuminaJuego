using UnityEngine;
using UnityEngine.UI;

// VidasUI controla la representación visual de las vidas en pantalla
// Cambia entre sprite de vida llena y vida vacía según las vidas actuales
// Este script va en el GameObject "Panel_Vidas" dentro del Canvas
public class VidasUI : MonoBehaviour
{
    [Header("Sprites de vida")]
    // Arrastra aquí el sprite del corazón LLENO desde el Project
    public Sprite spriteVidaLlena;

    // Arrastra aquí el sprite del corazón VACÍO/GRIS desde el Project
    public Sprite spriteVidaVacia;

    [Header("Imágenes de los corazones en pantalla")]
    // Arrastra aquí los 3 objetos Image desde el Hierarchy
    // El orden importa: corazones[0] = Corazon1, [1] = Corazon2, [2] = Corazon3
    public Image[] corazones;

    // Singleton para que el VidasManager pueda accederlo fácilmente
    public static VidasUI instancia;

    void Awake()
    {
        if (instancia == null)
            instancia = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Verificamos que tenemos todo lo necesario
        if (spriteVidaLlena == null || spriteVidaVacia == null)
        {
            Debug.LogError("VidasUI: Faltan sprites. Asígnalos en el Inspector.");
            return;
        }

        if (corazones == null || corazones.Length == 0)
        {
            Debug.LogError("VidasUI: No hay imágenes de corazones asignadas.");
            return;
        }

        // Mostramos todas las vidas llenas al iniciar
        ActualizarVidas(corazones.Length);
    }

    // ---- Actualiza visualmente los corazones según las vidas actuales ----
    // vidasActuales: cuántas vidas tiene el jugador ahora mismo
    // Llamado por el VidasManager cada vez que cambian las vidas
    public void ActualizarVidas(int vidasActuales)
    {
        // Recorremos todos los corazones
        for (int i = 0; i < corazones.Length; i++)
        {
            // Si este corazón corresponde a una vida que aún tiene → lleno
            // Si ya no tiene esa vida → vacío
            // Ejemplo con 2 vidas: i=0 lleno, i=1 lleno, i=2 vacío
            if (corazones[i] != null)
            {
                if (i < vidasActuales)
                {
                    // Esta vida todavía está disponible
                    corazones[i].sprite = spriteVidaLlena;

                    // Nos aseguramos que sea completamente visible
                    corazones[i].color = Color.white;
                }
                else
                {
                    // Esta vida ya se perdió
                    corazones[i].sprite = spriteVidaVacia;

                    // Opcional: lo hacemos semitransparente para más claridad
                    corazones[i].color = new Color(1f, 1f, 1f, 0.5f);
                }
            }
        }

        Debug.Log("VidasUI actualizada: " + vidasActuales + " vidas.");
    }

    // ---- Efecto visual al perder una vida ----
    // Hace que el corazón que se acaba de perder "parpadee" antes de vaciarse
    public void AnimarPerdidaVida(int vidasActuales)
    {
        // El índice del corazón que se acaba de perder
        // Ejemplo: si quedan 2 vidas, el corazón perdido es el índice 2
        int indiceCorazonPerdido = vidasActuales;

        // Verificamos que el índice es válido
        if (indiceCorazonPerdido >= 0 && indiceCorazonPerdido < corazones.Length)
        {
            // Iniciamos la corrutina de animación en ese corazón
            StartCoroutine(CorrutinaParpadeoCorazon(indiceCorazonPerdido, vidasActuales));
        }
        else
        {
            // Si el índice no es válido solo actualizamos directamente
            ActualizarVidas(vidasActuales);
        }
    }

    // ---- Corrutina que anima el corazón antes de vaciarlo ----
    System.Collections.IEnumerator CorrutinaParpadeoCorazon(int indice, int vidasFinales)
    {
        // Número de veces que parpadea antes de vaciarse
        int veces = 3;

        for (int i = 0; i < veces; i++)
        {
            // Ocultamos el corazón
            corazones[indice].color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(0.1f);

            // Mostramos el corazón
            corazones[indice].color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }

        // Después del parpadeo actualizamos todas las vidas
        ActualizarVidas(vidasFinales);
    }
}