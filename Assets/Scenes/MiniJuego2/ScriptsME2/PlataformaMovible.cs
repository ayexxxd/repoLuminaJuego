using UnityEngine;

public class PlataformaMovible : MonoBehaviour
{
    public Transform puntoFinal;

    public float velocidadMovimiento = 2f;
    public float velocidadRotacion = 100f;

    private bool debeMoverse = false;

    void Update()
    {
        if (debeMoverse)
        {
            MoverYRotarPlataforma();
        }
    }

    public void ActivarMovimiento()
    {
        debeMoverse = true;
    }

    void MoverYRotarPlataforma()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            puntoFinal.position,
            velocidadMovimiento * Time.deltaTime
        );

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            puntoFinal.rotation,
            velocidadRotacion * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, puntoFinal.position) < 0.05f &&
            Quaternion.Angle(transform.rotation, puntoFinal.rotation) < 0.5f)
        {
            transform.position = puntoFinal.position;
            transform.rotation = puntoFinal.rotation;
            debeMoverse = false;
        }
    }
}