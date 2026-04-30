using UnityEngine;
using UnityEngine.InputSystem;

namespace TopDown.Movement
{

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControl : MonoBehaviour
{
    public float moveSpeed;//velocidad de movimiento del jugador
    public Rigidbody2D rig;//referencia al componente para mover al jugador

    private float xInput = 0f;
    private float yInput = 0f;

    // Update is called once per frame
    void Update()
    {
        xInput = 0f;
        yInput = 0f;
        if(Keyboard.current.aKey.isPressed)//left
        xInput = -2f;

        else if(Keyboard.current.dKey.isPressed)//right
        xInput = 2f;

        if(Keyboard.current.wKey.isPressed)//up
        yInput = 2f;

        else if(Keyboard.current.sKey.isPressed)//down
        yInput = -2f;
    }
    public void FixedUpdate()
    {
        rig.linearVelocity = new Vector2(xInput * moveSpeed, yInput * moveSpeed);
    } 
}
}
