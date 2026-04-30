using UnityEngine;

namespace DefensoresDeSoftware
{
    public class ExInBullet : MonoBehaviour
    {
        [Header("Configuración de Vuelo")]
        public float speed = 15f;
        public float lifeTime = 2f; 
        
        [Header("Físicas")]
        public Rigidbody2D rig;

        [Header("Impacto")]
        public string targetTag = "Enemy"; 
        public int damage = 1;

        void Start()
        {
            // Iniciamos un temporizador para que la bala se destruya sola si no choca con nada
            Destroy(gameObject, lifeTime); 
        }

        public void Fire(Vector2 direction)
        {
            // Le damos la velocidad y dirección inicial a la bala
            rig.linearVelocity = direction * speed;
        }

        // Esta función se activa automáticamente cuando la bala toca otro objeto
        void OnTriggerEnter2D(Collider2D hitInfo)
        {
            // Verificamos si el objeto tocado es nuestro objetivo ("Player" o "Enemy")
            if (hitInfo.CompareTag(targetTag))
            {
                if (targetTag == "Player")
                {
                    // Buscamos el script del jugador en el objeto que tocamos
                    ExInPlayerControl playerScript = hitInfo.GetComponent<ExInPlayerControl>();
                    if (playerScript != null)
                    {
                        // El jugador se encarga de parpadear y restar su vida
                        playerScript.GetDamaged(damage); 
                    }
                    Destroy(gameObject);
                }
                // Busca esta sección dentro de OnTriggerEnter2D en ExInBullet.cs:
                else if (targetTag == "Enemy")
                {
                    // 1. Buscamos el script del enemigo en el objeto que tocamos
                    ExInEnemy enemyScript = hitInfo.GetComponent<ExInEnemy>();

                    if (enemyScript != null)
                    {
                        // 2. Le pedimos al enemigo que reciba 1 de daño
                        enemyScript.TakeDamage(damage);
                    }

                    // 3. Destruimos la bala (siempre se destruye al impactar)
                    Destroy(gameObject);         
                }
            }
        }
    }
}