using System;
using UnityEngine;
namespace TopDown.Shooting{//namespace para evitar conflictos de nombres
public class CameraController : MonoBehaviour
{
   [SerializeField] private Transform playerTransform;//referencia a jugador para seguirlo
   private float Zpos = -10;//posicion fija en Z para la camara
   private void Update()
    {
        //determine final camera position and assign it
        transform.position = new Vector3 (playerTransform.position.x, playerTransform.position.y , Zpos);
    }
//hi
}
}
