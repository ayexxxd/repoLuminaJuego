using UnityEngine;
public class EfectoVisualToxico : MonoBehaviour
{
    [Header("Configuración del pulso")]

    public float escalMaxima = 1.1f;

    public float escalaMinima = 0.9f;

    public float velocidadPulso = 2f;

    public Color colorBase = new Color(0.5f, 0f, 0.5f, 0.7f);
    public Color colorPulso = new Color(1f, 0f, 0.5f, 0.9f);


    private Vector3 escalaOriginal;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        escalaOriginal = transform.localScale;

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.color = colorBase;
        }
    }

    void Update()
    {

        float pulso = Mathf.PingPong(Time.time * velocidadPulso, 1f);

        float escalaActual = Mathf.Lerp(escalaMinima, escalMaxima, pulso);
        transform.localScale = escalaOriginal * escalaActual;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.Lerp(colorBase, colorPulso, pulso);
        }
    }
}