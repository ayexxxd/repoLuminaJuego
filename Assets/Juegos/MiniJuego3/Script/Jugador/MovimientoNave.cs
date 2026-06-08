using UnityEngine;
using UnityEngine.InputSystem;   

public class MovimientoNave : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float fuerzaMovimiento = 10f;
    private float multiplicadorVelocidad = 1f;

    public float velocidadGiro = 150f;
    [Header("Configuración de Drift")]
    public bool driftHabilitado = true;

    public float fuerzaDerrape = 0.85f;

    public float multiplicadorGiroDrift = 1.5f;
    private bool enDrift = false;

    private TrailRenderer rastro;

    public float velocidadMaxima = 8f;

    private Rigidbody2D rb;

    private float entradaMovimiento;  
    private float entradaGiro;    



    void Awake()
    {

        rb = GetComponent<Rigidbody2D>();

        rastro = GetComponent<TrailRenderer>();
    }

    void Update()
    {
        var kb = Keyboard.current;

        float arriba  = (kb.wKey.isPressed || kb.upArrowKey.isPressed)    ? 1f : 0f;
        float abajo   = (kb.sKey.isPressed || kb.downArrowKey.isPressed)  ? 1f : 0f;
        entradaMovimiento = arriba - abajo;

        float derecha = (kb.dKey.isPressed || kb.rightArrowKey.isPressed) ? 1f : 0f;
        float izquierda = (kb.aKey.isPressed || kb.leftArrowKey.isPressed) ? 1f : 0f;
        entradaGiro = derecha - izquierda;

        if (driftHabilitado)
        {
            enDrift = kb.leftShiftKey.isPressed || kb.spaceKey.isPressed;
        }

        if (rastro != null)
            rastro.emitting = enDrift;
    }

    void FixedUpdate()
    {
        MoverNave();

        if (enDrift)
            GirarNaveDrift();
        else
            GirarNave();

        LimitarVelocidad();

        if (enDrift)
            AplicarDerrape();
    }

    void MoverNave()
    {
        if (entradaMovimiento != 0)
        {
            Vector2 fuerza = transform.up * fuerzaMovimiento * entradaMovimiento * multiplicadorVelocidad;
            rb.AddForce(fuerza, ForceMode2D.Force);
        }
    }

    public void HitBoundary(Vector2 direccionRebote)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direccionRebote * 10f; 
        }
        
        Debug.Log("La nave chocó y rebotó");
    }

    void GirarNave()
    {
        if (entradaGiro != 0)
        {
            float cantidadGiro = -entradaGiro * velocidadGiro * Time.fixedDeltaTime;

            Quaternion rotacion = Quaternion.Euler(0, 0, cantidadGiro);

            rb.MoveRotation(rb.rotation + cantidadGiro);
        }
    }

    void LimitarVelocidad()
    {
        if (rb.linearVelocity.magnitude > velocidadMaxima)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * velocidadMaxima;
        }
    }
    public void AplicarEfectoVelocidad(float multiplicador, float duracion)
    {
        StopCoroutine("CorrutinaEfectoVelocidad");
        StartCoroutine(CorrutinaEfectoVelocidad(multiplicador, duracion));
    }

    System.Collections.IEnumerator CorrutinaEfectoVelocidad(float multiplicador, float duracion)
    {
        multiplicadorVelocidad = multiplicador;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            if (multiplicador < 1f)
            {
                sr.color = new Color(0.4f, 0.6f, 1f, 1f);
            }
            else if (multiplicador > 1f)
            {
                sr.color = new Color(1f, 0.9f, 0.2f, 1f);
            }
        }

        Debug.Log("Efecto de velocidad: x" + multiplicador + " por " + duracion + "s");
        yield return new WaitForSeconds(duracion);

        multiplicadorVelocidad = 1f;
        if (sr != null)
        {
            sr.color = Color.white;
        }

        Debug.Log("Velocidad restaurada.");
    }
    void AplicarDerrape()
    {
        Vector2 velocidadActual = rb.linearVelocity;

        Vector2 adelante = transform.up;

        float velocidadAdelante = Vector2.Dot(velocidadActual, adelante);

        Vector2 velocidadFrontal = adelante * velocidadAdelante;

        Vector2 velocidadLateral = velocidadActual - velocidadFrontal;

        rb.linearVelocity = velocidadFrontal + velocidadLateral * fuerzaDerrape;
    }

    void GirarNaveDrift()
    {
        if (entradaGiro != 0)
        {
            float cantidadGiro = -entradaGiro * velocidadGiro *
                                  multiplicadorGiroDrift * Time.fixedDeltaTime;
            rb.MoveRotation(rb.rotation + cantidadGiro);
        }
    }

                        
    }



