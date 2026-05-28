using UnityEngine;

public class Moneda : MonoBehaviour
{
    private ControlNivel controlNivel;

    void Start()
    {
        controlNivel = FindFirstObjectByType<ControlNivel>();
    }

    void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Player"))
    {
        controlNivel.RecolectarMoneda();

        if (AudioManager.instancia != null)
        {
            AudioManager.instancia.ReproducirMoneda();
        }

        gameObject.SetActive(false);
    }
}
}