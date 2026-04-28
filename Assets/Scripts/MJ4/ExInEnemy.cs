using UnityEngine;
using System.Collections;

namespace DefensoresDeSoftware
{
    public class ExInEnemy : MonoBehaviour
    {
        public float speed = 3f;
        public Rigidbody2D rig;
        public GameObject bulletPrefab;
        public Transform firePoint;
        public float fireRate = 2f; 
        
        // Lista de comportamientos que podemos elegir en Unity
        public enum TipoDisparo { Null, HaciaAdelante, Estrella }
        public TipoDisparo patronDisparo;

        void Start()
        {
            // Si el enemigo no es kamikaze (Null), encendemos su ciclo de disparo
            if (patronDisparo != TipoDisparo.Null)
            {
                StartCoroutine(RutinaDeDisparo());
            }
        }

        void FixedUpdate()
        {
            // El enemigo siempre avanza hacia la izquierda
            rig.linearVelocity = Vector2.left * speed;
        }

        // Hilo secundario que controla el ritmo de ataque
        IEnumerator RutinaDeDisparo()
        {
            // Bucle infinito: ataca mientras siga vivo
            while (true) 
            {
                // Espera el tiempo de recarga
                yield return new WaitForSeconds(fireRate);

                // Decide qué ataque usar según su configuración
                if (patronDisparo == TipoDisparo.HaciaAdelante) DisparoAdelante();
                else if (patronDisparo == TipoDisparo.Estrella) DisparoEstrella();
            }
        }

        void DisparoAdelante()
        {
            // Crea una bala y la lanza directamente hacia la izquierda (hacia el jugador)
            GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            ExInBullet bulletScript = newBullet.GetComponent<ExInBullet>();
            if (bulletScript != null) bulletScript.Fire(Vector2.left); 
        }

        void DisparoEstrella()
        {
            // Preparamos una lista con las 8 direcciones posibles
            Vector2[] direcciones = {
                Vector2.up, Vector2.down, Vector2.left, Vector2.right,
                new Vector2(1, 1).normalized, new Vector2(-1, 1).normalized,
                new Vector2(1, -1).normalized, new Vector2(-1, -1).normalized
            };

            // Disparamos una bala por cada dirección de la lista
            foreach (Vector2 dir in direcciones)
            {
                GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                ExInBullet bulletScript = newBullet.GetComponent<ExInBullet>();
                if (bulletScript != null) bulletScript.Fire(dir);
            }
        }

        // Detecta si choca físicamente (cuerpo a cuerpo) contra el jugador
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                // El enemigo se destruye al impactar
                Destroy(this.gameObject);
            }
        }
    }
}