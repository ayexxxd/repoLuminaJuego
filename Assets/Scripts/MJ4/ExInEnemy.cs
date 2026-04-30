using UnityEngine;
using System.Collections;

namespace DefensoresDeSoftware
{
    public class ExInEnemy : MonoBehaviour
    {
     
        [Header("Configuración Básica")]
        public int lives = 3; // Puedes cambiar este número en el Inspector de Unity
        public float knockbackFuerza = 5f;
        public int damageAlChocar = 1;
        public float speed = 3f;
        public Rigidbody2D rig;
        public GameObject bulletPrefab;
        public Transform firePoint;
        public float fireRate = 2f; 

        private float direccionX = -1f;     // -1 = Izquierda, 1 = Derecha

        [Header("Comportamientos")]
        
        public TipoDisparo patronDisparo;
        public enum TipoDisparo { Null, HaciaAdelante, Estrella, EnX }

        public enum TipoMovimiento { Estatico, Recto, Senoidal, Persecucion, PersecucionVectorial}
        public TipoMovimiento patronMovimiento = TipoMovimiento.Recto;
        
        public float velocidadOla = 5f;
        public float amplitudOla = 2f;
        public Transform playerTransform;

        [Header("División al Morir")]
        public bool seDivideAlMorir = false;
        public GameObject enemigoHijoPrefab;
        public int cantidadHijos = 2;
        public float fuerzaExplosionHijos = 5f;


        private SpriteRenderer spriteRenderer;
        private Vector2 inerciaActiva;
        private bool seEstaCerrandoElJuego = false;

        void Start()
        {
            // Unity busca el SpriteRenderer en este mismo objeto y lo guarda en la variable
            spriteRenderer = GetComponent<SpriteRenderer>();
            // Si el enemigo no es kamikaze (Null), encendemos su ciclo de disparo
            if (patronDisparo != TipoDisparo.Null)
            {
                StartCoroutine(RutinaDeDisparo());
            }

            if ((patronMovimiento == TipoMovimiento.Persecucion || patronMovimiento == TipoMovimiento.PersecucionVectorial) && playerTransform == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null) playerTransform = player.transform;
            }
        }

        public void RecibirInercia(Vector2 fuerza)
        {
            inerciaActiva = fuerza;
        }

        void FixedUpdate()
        {
            // 1. Calculamos la velocidad base deseada por el cerebro del enemigo
            Vector2 velocidadBase = Vector2.zero;

            if (patronMovimiento == TipoMovimiento.Estatico) 
            {
                velocidadBase = Vector2.zero;
            }
            else if (patronMovimiento == TipoMovimiento.Recto) 
            {
                velocidadBase = new Vector2(direccionX * speed, 0);
            }
            else if (patronMovimiento == TipoMovimiento.Senoidal)
            {
                float velocidadY = Mathf.Sin(Time.time * velocidadOla) * amplitudOla;
                velocidadBase = new Vector2(direccionX * speed, velocidadY);
            }
            else if (patronMovimiento == TipoMovimiento.Persecucion)
            {
                if (playerTransform != null)
                {
                    float direccionY = 0f;
                    if (playerTransform.position.y > transform.position.y) direccionY = 1f;
                    else if (playerTransform.position.y < transform.position.y) direccionY = -1f;
                    
                    velocidadBase = new Vector2(direccionX * speed, direccionY * (speed * 0.8f));
                }
                else 
                {
                    velocidadBase = new Vector2(direccionX * speed, 0);
                }
            }
            else if (patronMovimiento == TipoMovimiento.PersecucionVectorial)
            {
                if (playerTransform != null)
                {
                    // 1. Matemáticas: Destino - Origen = Dirección
                    Vector2 direccionHaciaJugador = (playerTransform.position - transform.position).normalized;
                    
                    // 2. Aplicamos la fuerza de empuje en esa dirección exacta
                    velocidadBase = direccionHaciaJugador * speed;

                    // Opcional: Descomenta estas dos líneas de abajo si quieres que el "dibujo" 
                    // del enemigo también rote para mirar físicamente hacia el jugador.
                    // float angulo = Mathf.Atan2(direccionHaciaJugador.y, direccionHaciaJugador.x) * Mathf.Rad2Deg;
                    // rig.MoveRotation(angulo);
                }
                else 
                {
                    // Si el jugador ya murió, sigue volando recto hacia donde estaba mirando
                    velocidadBase = new Vector2(direccionX * speed, 0);
                }
            }

            // 2. Fricción de la inercia (para la explosión de los hijos)
            inerciaActiva = Vector2.Lerp(inerciaActiva, Vector2.zero, Time.fixedDeltaTime * 5f);

            // 3. Sumamos la velocidad del cerebro + la inercia física
            Vector2 velocidadTotal = velocidadBase + inerciaActiva;

            // --- TU NUEVO SISTEMA ANTI-JITTERING ---

            // 4. Calculamos la posición futura imaginaria (igual que en el Player)
            Vector2 posicionFutura = rig.position + (velocidadTotal * Time.fixedDeltaTime);

            // 5. Sistema de Rebote en X: Evaluamos el futuro para reaccionar a tiempo
            if (posicionFutura.x <= ExInGameControl.Instance.minX)
            {
                direccionX = 1f; // Cambiamos de dirección
                posicionFutura.x = ExInGameControl.Instance.minX; // Lo pegamos exacto a la pared para que no se salga ni un pixel
            }
            else if (posicionFutura.x >= ExInGameControl.Instance.maxX)
            {
                direccionX = -1f;
                posicionFutura.x = ExInGameControl.Instance.maxX;
            }

            // 6. Límite estricto en Y (Muro invisible, igual que el Player)
            posicionFutura.y = Mathf.Clamp(posicionFutura.y, ExInGameControl.Instance.minY, ExInGameControl.Instance.maxY);

            // 7. Movemos el objeto de forma segura y suave
            rig.MovePosition(posicionFutura);
        }

        // Hilo secundario que controla el ritmo de ataque
        IEnumerator RutinaDeDisparo()
        {
            while (true) 
            {
                yield return new WaitForSeconds(fireRate);

                if (patronDisparo == TipoDisparo.HaciaAdelante) DisparoAdelante();
                else if (patronDisparo == TipoDisparo.Estrella) DisparoEstrella();
                else if (patronDisparo == TipoDisparo.EnX) DisparoEnX();
            }
        }

        void DisparoAdelante()
        {
            GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            ExInBullet bulletScript = newBullet.GetComponent<ExInBullet>();
            // La bala ahora se dispara hacia la dirección a la que esté mirando el enemigo
            if (bulletScript != null) bulletScript.Fire(new Vector2(direccionX, 0)); 
        }

        void DisparoEstrella()
        {
            Vector2[] direcciones = {
                Vector2.up, Vector2.down, Vector2.left, Vector2.right,
                new Vector2(1, 1).normalized, new Vector2(-1, 1).normalized,
                new Vector2(1, -1).normalized, new Vector2(-1, -1).normalized
            };

            foreach (Vector2 dir in direcciones)
            {
                GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                ExInBullet bulletScript = newBullet.GetComponent<ExInBullet>();
                if (bulletScript != null) bulletScript.Fire(dir);
            }
        }
        void DisparoEnX()
        {
            // Solo usamos las 4 esquinas (diagonales) y las normalizamos para que viajen a velocidad constante
            Vector2[] direcciones = {
                new Vector2(1, 1).normalized,   // Arriba a la derecha
                new Vector2(-1, 1).normalized,  // Arriba a la izquierda
                new Vector2(1, -1).normalized,  // Abajo a la derecha
                new Vector2(-1, -1).normalized  // Abajo a la izquierda
            };

            foreach (Vector2 dir in direcciones)
            {
                GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                ExInBullet bulletScript = newBullet.GetComponent<ExInBullet>();
                if (bulletScript != null) bulletScript.Fire(dir);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player")) 
            {
                // Buscamos el script del jugador en el objeto con el que chocamos
                ExInPlayerControl playerScript = collision.gameObject.GetComponent<ExInPlayerControl>();
                if (playerScript != null)
                {
                    // El jugador hace su flash rojo y procesa el daño
                    playerScript.GetDamaged(damageAlChocar);
                }

                // Ahora sí, destruimos a este enemigo
                Destroy(this.gameObject);
            }
        }
        

        public void TakeDamage(int damage)
        {
            lives -= damage; 
            
            if (lives <= 0)
            {
                // Si muere, lo destruimos y NO intentamos hacer el parpadeo
                Destroy(gameObject);
                return; // Esta línea es clave: detiene la función aquí mismo y no lee lo de abajo
            }

            // Si llegó hasta aquí, significa que sobrevivió al golpe.
            // Aplicamos el empuje y el parpadeo.
            RecibirInercia(Vector2.right * knockbackFuerza);

            if (spriteRenderer != null)
            {
                StartCoroutine(RedFlashEffect());
            }
        }

        // 3. Agrega esta nueva función al final del script:
        IEnumerator RedFlashEffect()
        {
            if (spriteRenderer != null) 
            {
                spriteRenderer.color = Color.red;
                yield return new WaitForSeconds(0.1f);
                spriteRenderer.color = Color.white;
            }
        }
        // Unity llama a esto automáticamente cuando cierras la ventana del juego
        void OnApplicationQuit()
        {
            seEstaCerrandoElJuego = true;
        }

        // Unity llama a esto 1 milisegundo antes de borrar el objeto de la memoria RAM
        void OnDestroy()
        {
            if (seEstaCerrandoElJuego || !gameObject.scene.isLoaded) 
                return;

            if (seDivideAlMorir && enemigoHijoPrefab != null)
            {
                for (int i = 0; i < cantidadHijos; i++)
                {
                    // --- NUEVA LÓGICA DE DIRECCIÓN ---
                    // Si 'i' es un número par (ej. 0), la Y es positiva (arriba). Si es impar (ej. 1), es negativa (abajo).
                    float direccionY = (i % 2 == 0) ? 1f : -1f; 
                    float direccionX = Random.Range(0f, 1f); 

                    Vector2 direccionCalculada = new Vector2(direccionX, direccionY).normalized;                   
                    // Calculamos dónde nacen usando nuestra nueva dirección
                    Vector2 spawnPos = (Vector2)transform.position + (direccionCalculada * 0.5f);

                    GameObject nuevoHijo = Instantiate(enemigoHijoPrefab, spawnPos, Quaternion.identity);

                    ExInEnemy scriptHijo = nuevoHijo.GetComponent<ExInEnemy>();
                    if (scriptHijo != null)
                    {
                        // Le inyectamos la inercia con la dirección que acabamos de construir
                        scriptHijo.RecibirInercia(direccionCalculada * fuerzaExplosionHijos);
                    }
                }
            }
        }
    }
}
