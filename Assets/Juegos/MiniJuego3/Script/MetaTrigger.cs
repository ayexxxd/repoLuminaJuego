using UnityEngine;

public class MetaTrigger : MonoBehaviour
{
    private LapManager lapManager;

    void Start()
    {
        lapManager = FindObjectOfType<LapManager>();

        Collider2D col = GetComponent<Collider2D>();

        if (col == null)
            Debug.LogError(gameObject.name + ": No tiene Collider2D.");
        else if (!col.isTrigger)
            Debug.LogError(gameObject.name + ": Activa isTrigger en el Collider2D.");

        if (lapManager == null)
            Debug.LogError("MetaTrigger: No se encontró LapManager en la escena.");
    }

    void OnTriggerEnter2D(Collider2D otro)
    {
        if (!otro.CompareTag("Jugador")) return;
        if (lapManager != null)
            lapManager.MetaCruzada();
    }
}