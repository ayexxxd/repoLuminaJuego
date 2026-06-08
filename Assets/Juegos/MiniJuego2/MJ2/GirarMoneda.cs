using UnityEngine;

public class GirarMoneda : MonoBehaviour
{
    public float velocidadGiro = 4f;
    public float anchoMinimo = 0.1f;

    private Vector3 escalaOriginal;

    void Start()
    {
        escalaOriginal = transform.localScale;
    }

    void Update()
    {
        float nuevoAncho = Mathf.Abs(Mathf.Sin(Time.time * velocidadGiro));

        if (nuevoAncho < anchoMinimo)
        {
            nuevoAncho = anchoMinimo;
        }

        transform.localScale = new Vector3(nuevoAncho * escalaOriginal.x, escalaOriginal.y, escalaOriginal.z);
    }
}