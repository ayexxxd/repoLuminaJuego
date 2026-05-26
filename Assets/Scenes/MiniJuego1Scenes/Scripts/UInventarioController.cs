// UIInventarioController.cs
// Muestra u oculta la UI de inventario y power-ups según el nivel.
// Ponlo en un GameObject vacío llamado "UIInventarioController" en GameScene.

using UnityEngine;

public class UIInventarioController : MonoBehaviour
{
    public static UIInventarioController instancia;

    [Header("Objetos a ocultar en nivel 1")]
    public GameObject[] objetosInventario;
    // Arrastra aquí:
    // - TextoMartillos
    // - TextoShuffles
    // - TextoMovExtras
    // - BotonMartillo
    // - BotonShuffle
    // - BotonMovExtra

    void Awake()
    {
        if (instancia == null) instancia = this;
    }

    void Start()
    {
        ActualizarVisibilidad();
    }

    public void ActualizarVisibilidad()
    {
        int nivel = PlayerPrefs.GetInt("NivelActual", 1);

        // En nivel 1 se oculta todo, en nivel 2+ se muestra
        bool mostrar = nivel >= 2;

        foreach (GameObject obj in objetosInventario)
        {
            if (obj != null)
                obj.SetActive(mostrar);
        }

        Debug.Log($"UI inventario — nivel:{nivel} visible:{mostrar}");
    }
}