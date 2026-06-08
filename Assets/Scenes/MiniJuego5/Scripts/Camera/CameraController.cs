using UnityEngine;

namespace TopDown.Shooting
{
public class CameraController : MonoBehaviour
{
   [SerializeField] private Transform playerTransform;
   private float Zpos = -10;
   private void Update()
   {
       transform.position = new Vector3 (playerTransform.position.x, playerTransform.position.y , Zpos);
   }
}
}
