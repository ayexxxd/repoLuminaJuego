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
                    // Le avisamos al gestor global que el jugador recibió daño
                    ExInGameControl.Instance.SpendLives();
                    // Destruimos esta bala
                    Destroy(gameObject);
                }
                else if (targetTag == "Enemy")
                {
                    // Destruimos al enemigo y luego a esta bala
                    Destroy(hitInfo.gameObject); 
                    Destroy(gameObject);         
                }
            }
        }
    }
}