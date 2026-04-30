using Unity.VisualScripting;
using UnityEngine;
namespace TopDown.Movement
{
public class Rotating : MonoBehaviour
{
   protected void LookAt(Vector3 target)
    {
        //calcula angulo entre jugador y objetivo
        float lookAngle = AngleBetweenPointsTwoPoints(transform.position, target);
        
        //rota el jugador hacia el objetivo, (+90 grados porque el sprite esta orientado mal)
        transform.eulerAngles = new Vector3(0,0,lookAngle + 90f);
    }

    private float AngleBetweenPointsTwoPoints(Vector3 a, Vector3 b)//funcion para calcular el angulo entre dos puntos
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}
}//hi i need to cmmit this or i will cry