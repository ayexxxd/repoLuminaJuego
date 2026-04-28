using System;
using UnityEngine;
namespace TopDown.Shooting{
public class CameraController : MonoBehaviour
{
   [SerializeField] private Transform playerTransform;
   private float Zpos = -10;
   private void Update()
    {
        //Determine final camera position and assign it
        transform.position = new Vector3 (playerTransform.position.x, playerTransform.position.y , Zpos);
    }
//hi
}
}
