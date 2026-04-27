using UnityEngine;
using UnityEngine.InputSystem;

namespace TopDown.Movement
{

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControl : MonoBehaviour
{
    public float moveSpeed ;
    public Rigidbody2D rig;

    private float xInput = 0f;
    private float yInput = 0f;
    public SpriteRenderer sr;
    private Animator animatorController;


//turning around
    void Start()
    {
        //animatorController = GetComponent<Animator>();

    }

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
        //UpdatePlayerAnimation();//animate
    }

    public void FixedUpdate()
    {
        rig.linearVelocity = new Vector2(xInput * moveSpeed, yInput * moveSpeed);
    } 
}
}
