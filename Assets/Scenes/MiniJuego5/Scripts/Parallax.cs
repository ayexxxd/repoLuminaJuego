using UnityEngine;

public class Parallax : MonoBehaviour//no es parallax real, es un sistema de repeticion de fondo
{
    public Transform player;//referencia al jugador
    private float length, height;//ancho del sprite para calcular el punto de repeticion

    void Start()
    {
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        height = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Update()
    {
        Vector3 pos = transform.position;
        Vector3 playerPos = player.position;

        //posicion horizontal
        if (playerPos.x > pos.x + length)
        {
            transform.position = new Vector3(pos.x + length * 2, pos.y, pos.z);
        }
        else if (playerPos.x < pos.x - length)
        {
            transform.position = new Vector3(pos.x - length * 2, pos.y, pos.z);
        }

        //posicion vertical
        if (playerPos.y > pos.y + height)
        {
            transform.position = new Vector3(transform.position.x, pos.y + height * 2, pos.z);
        }
        else if (playerPos.y < pos.y - height)
        {
            transform.position = new Vector3(transform.position.x, pos.y - height * 2, pos.z);
        }
    }
}