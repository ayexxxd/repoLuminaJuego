using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
   [SerializeField] private Transform playerTransform;
   [SerializeField] private float displacementMultiplier = 0.15f;
   private float Zpos = -10;
   private void Update()
    {
        //calculate mous position in wordl coords then calc displacemnet depeding on diff betrween mouse and player pos
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 cameraDisplacement = (mousePos - playerTransform.position) * displacementMultiplier;

        //Determine final camera position and assign it
        Vector3 finalCameraPos = playerTransform.position + cameraDisplacement;
        finalCameraPos.z = Zpos;
        transform.position = new Vector3 (playerTransform.position.x, playerTransform.position.y , Zpos);
    }


}
