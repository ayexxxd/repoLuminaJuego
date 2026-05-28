using UnityEngine;

public class GirarMonedaUI : MonoBehaviour
{
    public float velocidadGiro = 120f;

    void Update()
    {
        transform.Rotate(0f, velocidadGiro * Time.deltaTime, 0f);
    }
}