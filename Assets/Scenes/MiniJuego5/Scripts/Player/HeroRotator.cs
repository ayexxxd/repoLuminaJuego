using UnityEngine;
using UnityEngine.InputSystem;

namespace TopDown.Movement
{
public class HeroRotator : MonoBehaviour
{
    private void Awake()
    {
        if (TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.freezeRotation = true;
        }
    }

    private void OnLook(InputValue value)//funcion para rotar al jugador hacia el mouse
    {   
        Vector2 mouseScreen = value.Get<Vector2>();//obtener posicion del mouse en pantalla
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);//convertir posicion del mouse a coordenadas

        LookAt(mouseWorld);//rotar hacia la posicion del mouse
}   
   protected void LookAt(Vector3 target)
    {
        //calcula angulo entre jugador y objetivo
        float lookAngle = AngleBetweenPointsTwoPoints(transform.position, target);
        
        //rota el jugador hacia el objetivo, (+90 grados porque el sprite esta orientado mal)
        transform.eulerAngles = new Vector3(0,0,lookAngle + 90f);
    }

    private float AngleBetweenPointsTwoPoints(Vector3 a, Vector3 b)//funcion trig para calcular el angulo entre dos puntos
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}
}//hi i need to cmmit this or i will cry