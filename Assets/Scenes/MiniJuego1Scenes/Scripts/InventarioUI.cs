using UnityEngine;
using TMPro;

public class InventarioUI : MonoBehaviour
{
    [Header("Textos del inventario")]
    public TextMeshProUGUI textoMartillos;
    public TextMeshProUGUI textoShuffles;
    public TextMeshProUGUI textoMovExtras;

    void Start()
    {
        if (InventarioManager.instancia == null) return;

        InventarioManager.instancia.AsignarTextos(
            textoMartillos, textoShuffles, textoMovExtras);
    }
}