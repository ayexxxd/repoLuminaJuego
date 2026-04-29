using UnityEngine;
using System.Collections;

namespace DefensoresDeSoftware
{
    public class ExInEnemy : MonoBehaviour
    {
        [Header("Movimiento")]
        public float speed = 3f;
        public Rigidbody2D rig;

        // Añadimos el estado 'Estatico' a la lista de opciones
        public enum TipoMovimiento { Estatico, Recto, Senoidal, Persecucion }
        public TipoMovimiento patronMovimiento;

        [Header("Ajustes Extra de Movimiento")]
        public float amplitudOla = 3f;   
        public float velocidadOla = 5f;  

        [Header("Disparo")]
        public GameObject bulletPrefab;
        public Transform firePoint;
        public float fireRate = 2f; 
        public enum TipoDisparo { Null, HaciaAdelante, Estrella }
        public TipoDisparo patronDisparo;

        [Header("Al Morir (División)")]
        public bool seDivideAlMorir = false;
        public GameObject enemigoHijoPrefab; 
        public int cantidadHijos = 2;        
        public float fuerzaExplosionHijos = 15f; // Qué tan violento es el empuje

        

        // Privadas ----------

        private Transform playerTransform; 
        private bool seEstaCerrandoElJuego = false;
        // Memoria temporal del impacto físico
        private Vector2 inerciaActiva = Vector2.zero;

        void Start()
        {
            // Solo buscamos al jugador si el enemigo es del tipo Persecución
            if (patronMovimiento == TipoMovimiento.Persecucion)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    playerTransform = player.transform;
                }
            }

            // Si el enemigo dispara, iniciamos su temporizador
            if (patronDisparo != TipoDisparo.Null)
            {
                StartCoroutine(RutinaDeDisparo());
            }
        }

        void FixedUpdate()
        {
            // Decidimos cómo se mueve este enemigo
            if (patronMovimiento == TipoMovimiento.Estatico)
            {
                // El Sniper no se mueve, anulamos cualquier velocidad
                rig.linearVelocity = Vector2.zero;
            }
            else if (patronMovimiento == TipoMovimiento.Recto)
            {
                // Avanza directo a la izquierda
                rig.linearVelocity = Vector2.left * speed;
            }
            else if (patronMovimiento == TipoMovimiento.Senoidal)
            {
                // Avanza a la izquierda oscilando en forma de ola
                float velocidadY = Mathf.Sin(Time.time * velocidadOla) * amplitudOla;
                rig.linearVelocity = new Vector2(-speed, velocidadY);
            }
            else if (patronMovimiento == TipoMovimiento.Persecucion)
            {
                // Rastrea la altura del jugador y avanza a la izquierda
                if (playerTransform != null)
                {
                    float direccionY = 0f;
                    if (playerTransform.position.y > transform.position.y) direccionY = 1f;
                    else if (playerTransform.position.y < transform.position.y) direccionY = -1f;

                    rig.linearVelocity = new Vector2(-speed, direccionY * (speed * 0.8f));
                }
                else
                {
                    rig.linearVelocity = Vector2.left * speed;
                }
            }
        }

        // --- (El código de disparo queda igual) ---
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
            // La bala del enemigo nace rotada según como hayas girado su propio FirePoint
            GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            
            ExInBullet bulletScript = newBullet.GetComponent<ExInBullet>();
            if (bulletScript != null) bulletScript.Fire(Vector2.left); 
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
            if (collision.gameObject.CompareTag("Player")) Destroy(this.gameObject);
        }
        // --- NUEVO: GESTIÓN DE DESTRUCCIÓN ---

        // Unity llama a esto automáticamente cuando cierras la ventana del juego
        void OnApplicationQuit()
        {
            seEstaCerrandoElJuego = true;
        }

        // Unity llama a esto 1 milisegundo antes de borrar el objeto de la memoria RAM
        void OnDestroy()
        {
            // CANDADO DE SEGURIDAD: Evita generar enemigos si el juego se está cerrando
            // o si estamos cambiando a la pantalla de Game Over.
            if (seEstaCerrandoElJuego || !gameObject.scene.isLoaded) return;

            if (seDivideAlMorir && enemigoHijoPrefab != null)
            {
                for (int i = 0; i < cantidadHijos; i++)
                {
                    // Añadimos un pequeño factor aleatorio para que los hijos no nazcan 
                    // fusionados en el mismo pixel exacto.
                    Vector2 randomOffset = new Vector2(Random.Range(-0.5f, 0.5f), -1f * (i + 1));
                    Vector2 spawnPos = (Vector2)transform.position + randomOffset;

                    Instantiate(enemigoHijoPrefab, spawnPos, Quaternion.identity);
                }
            }
        }
    }
}