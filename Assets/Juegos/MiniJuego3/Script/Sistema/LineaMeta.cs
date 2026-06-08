using UnityEngine;

public class LineaMeta : MonoBehaviour
{
    private LapManager lapManager;

    void Start()
    {
        lapManager = FindObjectOfType<LapManager>();

        Collider2D col = GetComponent<Collider2D>();

        if (col == null)
        {
            Debug.LogError("LineaMeta: No tiene Collider2D.");
            return;
        }

        if (!col.isTrigger)
        {
            Debug.LogError("LineaMeta: El Collider2D NO tiene isTrigger activado.");
        }

        if (lapManager == null)
        {
            Debug.LogError("LineaMeta: No se encontró el LapManager en la escena.");
            return;
        }

        Debug.Log("LineaMeta: lista y conectada al LapManager.");
    }

    void OnTriggerEnter2D(Collider2D otro)
    {
        Debug.Log("LineaMeta: algo cruzó → " + otro.gameObject.name +
                " (tag: " + otro.tag + ")");

        if (!otro.CompareTag("Jugador")) return;

        Debug.Log(" ¡Jugador cruzó la meta!");

        if (lapManager != null)
            lapManager.MetaCruzada();
    }
}