// InventarioUI.cs
// Ponlo en un GameObject vacío en GameScene.
// Conecta los textos en el Inspector y los pasa al InventarioManager.

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

        // Le pasa las referencias y pide actualizar
        InventarioManager.instancia.AsignarTextos(
            textoMartillos, textoShuffles, textoMovExtras);
    }
}