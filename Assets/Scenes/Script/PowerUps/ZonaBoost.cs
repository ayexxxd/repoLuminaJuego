using UnityEngine;

public class ZonaBoost : MonoBehaviour
{
    [Header("Efecto visual")]

    public Color colorActivo = new Color(1f, 0.9f, 0.2f, 0.9f);
    public Color colorNormal = new Color(1f, 0.9f, 0.2f, 0.5f);

    public float velocidadRotacion = 45f;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.color = colorNormal;
        }
    }

    void Update()
    {
        transform.Rotate(0, 0, velocidadRotacion * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D otro)
    {
        if (!otro.CompareTag("Jugador")) return;

        Debug.Log("¡Nave entró a zona boost!");

        if (spriteRenderer != null)
        {
            spriteRenderer.color = colorActivo;
        }

        if (PowerUpManager.instancia != null)
        {
            PowerUpManager.instancia.AplicarBoost();
        }
    }

    void OnTriggerExit2D(Collider2D otro)
    {
        if (!otro.CompareTag("Jugador")) return;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = colorNormal;
        }
    }
}