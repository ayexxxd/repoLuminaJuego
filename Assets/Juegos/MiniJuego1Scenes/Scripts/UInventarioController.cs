using UnityEngine;

public class UIInventarioController : MonoBehaviour
{
    public static UIInventarioController instancia;

    [Header("Objetos a ocultar en nivel 1")]
    public GameObject[] objetosInventario;

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

        bool mostrar = nivel >= 2;

        foreach (GameObject obj in objetosInventario)
        {
            if (obj != null)
                obj.SetActive(mostrar);
        }

        Debug.Log($"UI inventario — nivel:{nivel} visible:{mostrar}");
    }
}