using UnityEngine;

public class PowerUpEscudo : MonoBehaviour
{
    public float rotacion = 80f;

    void Update()
    {
        transform.Rotate(0f, 0f, rotacion * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D otro)
    {
        Debug.Log("Algo tocó el powerup: " + otro.name);

        // Buscar SIEMPRE el objeto raíz
        GameObject jugador = otro.transform.root.gameObject;

        // Revisamos tag del jugador
        if (jugador.CompareTag("Jugador"))
        {
            Debug.Log(" Jugador detectado");

            EscudoTemporal escudo =
                jugador.GetComponent<EscudoTemporal>();

            if (escudo != null)
            {
                escudo.ActivarEscudo();

                Debug.Log(" Escudo activado");
            }
            else
            {
                Debug.LogError(" No se encontró EscudoTemporal");
            }

            Destroy(gameObject);
        }
    }
}