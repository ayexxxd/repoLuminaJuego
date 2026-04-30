using UnityEngine;
using UnityEngine.InputSystem;

namespace TopDown.Movement
{
public class HeroRotator : Rotating
{
   private void OnLook(InputValue value)
    {   
        Vector2 mouseScreen = value.Get<Vector2>();
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);

        LookAt(mouseWorld);
}   
}
}
