using UnityEngine;
using UnityEngine.InputSystem;

public class JugadorMovimiento : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 8f;

    public float alturaAgachado = 0.5f;

    private Rigidbody2D rb;
    private CapsuleCollider2D colisionPersonaje;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float movimientoActual;

    private bool estaEnPiso = false;

    private Vector2 tamañoOriginalCollider;
    private Vector2 offsetOriginalCollider;
    
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        colisionPersonaje = GetComponent<CapsuleCollider2D>();

        animator = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        tamañoOriginalCollider = colisionPersonaje.size;
        offsetOriginalCollider = colisionPersonaje.offset;
    }

    void Update()
    {
        Mover();
        Saltar();
        Agacharse();
        ActualizarAnimaciones();
    }

    void Mover()
    {
        movimientoActual = 0f;

        if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
        {
            movimientoActual = -1f;
        }
        else if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
        {
            movimientoActual = 1f;
        }

        rb.linearVelocity = new Vector2(movimientoActual * velocidad, rb.linearVelocity.y);
    }
    void Saltar()
    {
        if ((Keyboard.current.upArrowKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame) && estaEnPiso)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
        }
    }

    void Agacharse()
    {
        if (Keyboard.current.downArrowKey.isPressed || Keyboard.current.sKey.isPressed)
        {
            colisionPersonaje.size = new Vector2(tamañoOriginalCollider.x, tamañoOriginalCollider.y * alturaAgachado);

            colisionPersonaje.offset = new Vector2(offsetOriginalCollider.x, offsetOriginalCollider.y - 0.3f);
        }
        else
        {
            colisionPersonaje.size = tamañoOriginalCollider;

            colisionPersonaje.offset = offsetOriginalCollider;
        }
    }

    void ActualizarAnimaciones()
    {
        float velocidadHorizontal = Mathf.Abs(rb.linearVelocity.x);

        bool estaCaminando = velocidadHorizontal > 0.1f;

        bool estaAgachado = Keyboard.current.downArrowKey.isPressed || Keyboard.current.sKey.isPressed;

        bool estaSaltando = estaEnPiso == false;

        animator.SetBool("isWalking", estaCaminando);

        animator.SetBool("isJumping", estaSaltando);

        animator.SetBool("isCrouching", estaAgachado);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Piso"))
        {
            estaEnPiso = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Piso"))
        {
            estaEnPiso = false;
        }
    }

    void LateUpdate()
    {
        if (movimientoActual < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movimientoActual > 0)
        {
            spriteRenderer.flipX = false;
        }
    }
}

    