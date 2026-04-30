using UnityEngine;
using UnityEngine.InputSystem;

namespace TopDown.Movement
{
public class HeroRotator : Rotating
{
   private void OnLook(InputValue value)//funcion para rotar al jugador hacia el mouse
    {   
        Vector2 mouseScreen = value.Get<Vector2>();//obtener posicion del mouse en pantalla
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);//convertir posicion del mouse a coordenadas

        LookAt(mouseWorld);//rotar hacia la posicion del mouse
}   
}
}
