using UnityEngine;
using UnityEngine.InputSystem;   

// Este script controla el movimiento de la nave del jugador
// Se comunica con el Rigidbody2D para mover la nave usando física real
public class MovimientoNave : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    // Fuerza con la que la nave acelera hacia adelante o atrás
    public float fuerzaMovimiento = 10f;
    private float multiplicadorVelocidad = 1f;

    // Velocidad con la que la nave gira (en grados por segundo)
    public float velocidadGiro = 150f;
    [Header("Configuración de Drift")]
// Si el drift está habilitado
    public bool driftHabilitado = true;

    // Cuánto resbala la nave durante el drift (0 = nada, 1 = máximo)
    public float fuerzaDerrape = 0.85f;

    // Multiplicador de giro durante el drift
    public float multiplicadorGiroDrift = 1.5f;

    // Si está en modo drift ahora mismo
    private bool enDrift = false;

    // Referencia al TrailRenderer para el rastro visual
    private TrailRenderer rastro;

    // Velocidad máxima que puede alcanzar la nave
    public float velocidadMaxima = 8f;

    // ---- Variables privadas (internas del script) ----

    // Referencia al componente Rigidbody2D de la nave
    private Rigidbody2D rb;

    // Aquí guardamos qué teclas está presionando el jugador
    private float entradaMovimiento;   // -1 = atrás, 0 = nada, 1 = adelante
    private float entradaGiro;         // -1 = derecha, 1 = izquierda


    // ---- Unity llama a Awake() cuando el objeto aparece en la escena ----
    void Awake()
    {
        // Buscamos y guardamos el componente Rigidbody2D que está en este mismo GameObject
        // GetComponent<>() busca un componente específico en el objeto
        rb = GetComponent<Rigidbody2D>();
        // Buscamos el TrailRenderer si existe
        rastro = GetComponent<TrailRenderer>();
    }

    // ---- Unity llama a Update() una vez por cada frame ----
    // Aquí leemos las teclas del jugador (input)
    void Update()
    {
        var kb = Keyboard.current;

        // Vertical: W / ↑ = adelante,  S / ↓ = atrás
        float arriba  = (kb.wKey.isPressed || kb.upArrowKey.isPressed)    ? 1f : 0f;
        float abajo   = (kb.sKey.isPressed || kb.downArrowKey.isPressed)  ? 1f : 0f;
        entradaMovimiento = arriba - abajo;

        // Horizontal: D / → = derecha,  A / ← = izquierda
        float derecha = (kb.dKey.isPressed || kb.rightArrowKey.isPressed) ? 1f : 0f;
        float izquierda = (kb.aKey.isPressed || kb.leftArrowKey.isPressed) ? 1f : 0f;
        entradaGiro = derecha - izquierda;

        // Drift: Shift izquierdo o Espacio
        if (driftHabilitado)
        {
            enDrift = kb.leftShiftKey.isPressed || kb.spaceKey.isPressed;
        }

        if (rastro != null)
            rastro.emitting = enDrift;
    }

    // ---- Unity llama a FixedUpdate() a intervalos fijos de tiempo ----
    // Todo lo que involucra física DEBE ir aquí, no en Update()
    void FixedUpdate()
    {
        MoverNave();

        // Si está en drift usamos el giro especial
        if (enDrift)
            GirarNaveDrift();
        else
            GirarNave();

        LimitarVelocidad();

        // Aplicamos el efecto de derrape si está en drift
        if (enDrift)
            AplicarDerrape();
    }

    // ---- Mueve la nave hacia adelante o hacia atrás ----
    void MoverNave()
    {
        // Solo aplicamos fuerza si el jugador está presionando una tecla de movimiento
        if (entradaMovimiento != 0)
        {
            // transform.up es la dirección "arriba" del objeto según su rotación actual
            // Si la nave gira, transform.up gira con ella → la nave siempre avanza hacia su frente
            Vector2 fuerza = transform.up * fuerzaMovimiento * entradaMovimiento * multiplicadorVelocidad;
            // AddForce() le aplica una fuerza al Rigidbody2D
            // ForceMode2D.Force aplica fuerza de forma continua y suave
            rb.AddForce(fuerza, ForceMode2D.Force);
        }
    }

    public void HitBoundary(Vector2 direccionRebote)
    {
        // Aquí es donde defines qué hace la nave al chocar
        // Por ejemplo, si usas un Rigidbody2D:
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Esto le da un empujón a la nave en la dirección calculada
            rb.linearVelocity = direccionRebote * 10f; 
        }
        
        Debug.Log("La nave chocó y rebotó");
    }

    // ---- Gira la nave a izquierda o derecha ----
    void GirarNave()
    {
        // Solo giramos si el jugador presiona izquierda o derecha
        if (entradaGiro != 0)
        {
            // Calculamos cuánto giramos en este frame
            // Time.fixedDeltaTime asegura que el giro sea igual sin importar la velocidad del juego
            float cantidadGiro = -entradaGiro * velocidadGiro * Time.fixedDeltaTime;

            // Creamos una rotación en el eje Z (el único que usamos en 2D)
            Quaternion rotacion = Quaternion.Euler(0, 0, cantidadGiro);

            // Aplicamos la rotación multiplicando la rotación actual por la nueva
            rb.MoveRotation(rb.rotation + cantidadGiro);
        }
    }

    // ---- Limita la velocidad máxima de la nave ----
    void LimitarVelocidad()
    {
        // rb.linearVelocity es un Vector2 con la velocidad actual en X e Y
        // .magnitude es la longitud de ese vector = la velocidad total
        if (rb.linearVelocity.magnitude > velocidadMaxima)
        {
            // .normalized devuelve el mismo vector pero con longitud 1
            // Lo multiplicamos por velocidadMaxima para cortar la velocidad al máximo permitido
            rb.linearVelocity = rb.linearVelocity.normalized * velocidadMaxima;
        }
    }
    // ---- Aplica un efecto de velocidad temporal a la nave ----
    // Llamado por obstáculos y power-ups
    // multiplicador: 0.3 = lento, 1 = normal, 1.5 = boost
    // duracion: cuántos segundos dura el efecto
    public void AplicarEfectoVelocidad(float multiplicador, float duracion)
    {
        // Si ya hay un efecto activo, lo cancelamos antes de aplicar el nuevo
        StopCoroutine("CorrutinaEfectoVelocidad");
        StartCoroutine(CorrutinaEfectoVelocidad(multiplicador, duracion));
    }

    // ---- Corrutina que aplica el efecto y lo revierte después ----
    System.Collections.IEnumerator CorrutinaEfectoVelocidad(float multiplicador, float duracion)
    {
        multiplicadorVelocidad = multiplicador;

        // Obtenemos el SpriteRenderer para cambiar el color
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        // Si es un efecto de lentitud, ponemos la nave de color azul
        // Si es un boost, la ponemos de color amarillo
        if (sr != null)
        {
            if (multiplicador < 1f)
            {
                // Lento — color azul
                sr.color = new Color(0.4f, 0.6f, 1f, 1f);
            }
            else if (multiplicador > 1f)
            {
                // Boost — color amarillo
                sr.color = new Color(1f, 0.9f, 0.2f, 1f);
            }
        }

        Debug.Log("Efecto de velocidad: x" + multiplicador + " por " + duracion + "s");
        yield return new WaitForSeconds(duracion);

        // Restauramos el color y la velocidad normal
        multiplicadorVelocidad = 1f;
        if (sr != null)
        {
            sr.color = Color.white;
        }

        Debug.Log("Velocidad restaurada.");
    }
    // ---- Aplica el efecto de resbalamiento durante el drift ----
    // ---- Aplica el efecto de resbalamiento durante el drift ----
    void AplicarDerrape()
    {
        // Reducimos la velocidad perpendicular a la nave
        // Esto crea el efecto de que la nave "resbala" de lado
        Vector2 velocidadActual = rb.linearVelocity;

        // Dirección hacia adelante de la nave
        Vector2 adelante = transform.up;

        // Componente de velocidad hacia adelante
        float velocidadAdelante = Vector2.Dot(velocidadActual, adelante);

        // Velocidad hacia adelante como vector
        Vector2 velocidadFrontal = adelante * velocidadAdelante;

        // Velocidad lateral (la que causa el derrape)
        Vector2 velocidadLateral = velocidadActual - velocidadFrontal;

        // Reducimos la velocidad lateral según la fuerza de derrape
        // fuerzaDerrape cerca de 1 = mucho derrape / cerca de 0 = poco
        rb.linearVelocity = velocidadFrontal + velocidadLateral * fuerzaDerrape;
    }

    // ---- Versión especial del giro durante drift ----
    void GirarNaveDrift()
    {
        if (entradaGiro != 0)
        {
            // Durante el drift la nave gira más rápido
            float cantidadGiro = -entradaGiro * velocidadGiro *
                                  multiplicadorGiroDrift * Time.fixedDeltaTime;
            rb.MoveRotation(rb.rotation + cantidadGiro);
        }
    }

        

                
    }



