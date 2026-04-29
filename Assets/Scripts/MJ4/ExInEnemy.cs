using UnityEngine;
using System.Collections;

namespace DefensoresDeSoftware
{
    public class ExInEnemy : MonoBehaviour
    {
     
        [Header("Configuración Básica")]
        public float speed = 3f;
        public Rigidbody2D rig;
        public GameObject bulletPrefab;
        public Transform firePoint;
        public float fireRate = 2f; 

        [Header("Límites de Pantalla (Rebote)")]
        public float limiteIzquierdo = -9f; // Ajusta esto en el Inspector según tu cámara
        public float limiteDerecho = 9f;    // Ajusta esto en el Inspector según tu cámara
        private float direccionX = -1f;     // -1 = Izquierda, 1 = Derecha

        [Header("Comportamientos")]
        
        public TipoDisparo patronDisparo;
        public enum TipoDisparo { Null, HaciaAdelante, Estrella }

        public enum TipoMovimiento { Estatico, Recto, Senoidal, Persecucion }
        public TipoMovimiento patronMovimiento = TipoMovimiento.Recto;
        
        public float velocidadOla = 5f;
        public float amplitudOla = 2f;
        public Transform playerTransform;

        [Header("División al Morir")]
        public bool seDivideAlMorir = false;
        public GameObject enemigoHijoPrefab;
        public int cantidadHijos = 2;
        public float fuerzaExplosionHijos = 5f;

        private Vector2 inerciaActiva;
        private bool seEstaCerrandoElJuego = false;

        void Start()
        {
            // Si el enemigo no es kamikaze (Null), encendemos su ciclo de disparo
            if (patronDisparo != TipoDisparo.Null)
            {
                StartCoroutine(RutinaDeDisparo());
            }

            // Si es de persecución y no le asignamos al jugador, lo busca automáticamente
            if (patronMovimiento == TipoMovimiento.Persecucion && playerTransform == null)
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
            // --- NUEVO: SISTEMA DE REBOTE ---
            // Si toca o cruza el límite izquierdo, lo forzamos a ir a la derecha
            if (transform.position.x <= limiteIzquierdo)
            {
                direccionX = 1f; 
            }
            // Si toca o cruza el límite derecho, lo forzamos a ir a la izquierda
            else if (transform.position.x >= limiteDerecho)
            {
                direccionX = -1f;
            }

            // 1. Calculamos a dónde quiere ir el cerebro del enemigo
            Vector2 velocidadBase = Vector2.zero;

            if (patronMovimiento == TipoMovimiento.Estatico) 
            {
                velocidadBase = Vector2.zero;
            }
            else if (patronMovimiento == TipoMovimiento.Recto) 
            {
                // Ahora usa "direccionX" en lugar de siempre ir a la izquierda
                velocidadBase = new Vector2(direccionX * speed, 0);
            }
            else if (patronMovimiento == TipoMovimiento.Senoidal)
            {
                float velocidadY = Mathf.Sin(Time.time * velocidadOla) * amplitudOla;
                // Aplica el rebote horizontal y el movimiento de ola vertical
                velocidadBase = new Vector2(direccionX * speed, velocidadY);
            }
            else if (patronMovimiento == TipoMovimiento.Persecucion)
            {
                if (playerTransform != null)
                {
                    float direccionY = 0f;
                    if (playerTransform.position.y > transform.position.y) direccionY = 1f;
                    else if (playerTransform.position.y < transform.position.y) direccionY = -1f;
                    
                    // Persigue en Y, pero respeta el rebote en X
                    velocidadBase = new Vector2(direccionX * speed, direccionY * (speed * 0.8f));
                }
                else 
                {
                    velocidadBase = new Vector2(direccionX * speed, 0);
                }
            }

            // 2. Fricción simulada: Reducimos la inercia un 5% cada fotograma físico
            inerciaActiva = Vector2.Lerp(inerciaActiva, Vector2.zero, Time.fixedDeltaTime * 5f);

            // 3. Resultado final: El cerebro + El impacto físico
            rig.linearVelocity = velocidadBase + inerciaActiva;
        }

        // Hilo secundario que controla el ritmo de ataque
        IEnumerator RutinaDeDisparo()
        {
            while (true) 
            {
                yield return new WaitForSeconds(fireRate);

                if (patronDisparo == TipoDisparo.HaciaAdelante) DisparoAdelante();
                else if (patronDisparo == TipoDisparo.Estrella) DisparoEstrella();
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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // El enemigo muere si choca con el jugador
            if (collision.gameObject.CompareTag("Player")) 
            {
                Destroy(this.gameObject);
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
            if (seEstaCerrandoElJuego || !gameObject.scene.isLoaded) return;

            if (seDivideAlMorir && enemigoHijoPrefab != null)
            {
                for (int i = 0; i < cantidadHijos; i++)
                {
                    Vector2 direccionAleatoria = Random.insideUnitCircle.normalized;
                    Vector2 spawnPos = (Vector2)transform.position + (direccionAleatoria * 0.5f);

                    GameObject nuevoHijo = Instantiate(enemigoHijoPrefab, spawnPos, Quaternion.identity);

                    ExInEnemy scriptHijo = nuevoHijo.GetComponent<ExInEnemy>();
                    if (scriptHijo != null)
                    {
                        scriptHijo.RecibirInercia(direccionAleatoria * fuerzaExplosionHijos);
                    }
                }
            }
        }
    }
}
