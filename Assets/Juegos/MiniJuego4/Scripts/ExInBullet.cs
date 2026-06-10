using UnityEngine;

namespace DefensoresDeSoftware
{
    public class ExInBullet : MonoBehaviour
    {
        [Header("Configuración de Vuelo")]
        public float speed = 15f;
        public float lifeTime = 3f; 
        
        [Header("Físicas")]
        public Rigidbody2D rig;

        [Header("Impacto")]
        public string targetTag = "Enemy"; 
        public int damage = 1;

        void Start()
        {

            Destroy(gameObject, lifeTime); 
        }

        public void Fire(Vector2 direction)
        {

            rig.linearVelocity = direction * speed;
        }

        void OnTriggerEnter2D(Collider2D hitInfo)
        {

            if (hitInfo.CompareTag(targetTag))
            {
                if (targetTag == "Player")
                {

                    ExInPlayerControl playerScript = hitInfo.GetComponent<ExInPlayerControl>();
                    if (playerScript != null)
                    {

                        playerScript.GetDamaged(damage); 
                    }
                    Destroy(gameObject);
                }

                else if (targetTag == "Enemy")
                {

                    ExInEnemy enemyScript = hitInfo.GetComponent<ExInEnemy>();

                    if (enemyScript != null)
                    {

                        enemyScript.TakeDamage(damage);
                    }

                    Destroy(gameObject);         
                }
            }
        }
    }
}