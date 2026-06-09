using UnityEngine;
using System.Collections;

namespace DefensoresDeSoftware
{
    public class ExInEnemy : MonoBehaviour
    {
     
        [Header("Configuración Básica")]
        public int lives = 3;
        public float knockbackFuerza = 5f;
        public int damageAlChocar = 1;
        public float speed = 3f;
        public Rigidbody2D rig;
        public GameObject bulletPrefab;
        public Transform firePoint;
        public float fireRate = 2f;
        public AudioClip shootSFX;

        

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

        private float direccionX = -1f;
        private SpriteRenderer spriteRenderer;
        private Vector2 inerciaActiva;
        private bool seEstaCerrandoElJuego = false;

        void Start()
        {

            spriteRenderer = GetComponent<SpriteRenderer>();

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

                    Vector2 direccionHaciaJugador = (playerTransform.position - transform.position).normalized;
                    

                    velocidadBase = direccionHaciaJugador * speed;

                }
                else 
                {

                    velocidadBase = new Vector2(direccionX * speed, 0);
                }
            }

            inerciaActiva = Vector2.Lerp(inerciaActiva, Vector2.zero, Time.fixedDeltaTime * 5f);

            Vector2 velocidadTotal = velocidadBase + inerciaActiva;

            Vector2 posicionFutura = rig.position + (velocidadTotal * Time.fixedDeltaTime);

            if (posicionFutura.x <= ExInGameControl.Instance.minX)
            {
                direccionX = 1f;
                posicionFutura.x = ExInGameControl.Instance.minX;
            }
            else if (posicionFutura.x >= ExInGameControl.Instance.maxX)
            {
                direccionX = -1f;
                posicionFutura.x = ExInGameControl.Instance.maxX;
            }

            posicionFutura.y = Mathf.Clamp(posicionFutura.y, ExInGameControl.Instance.minY, ExInGameControl.Instance.maxY);

            rig.MovePosition(posicionFutura);
        }

        IEnumerator RutinaDeDisparo()
        {
            while (true)
            {
                yield return new WaitForSeconds(fireRate);

                if (ExInSFXManager.Instance != null)
                    ExInSFXManager.Instance.PlaySFX(shootSFX, randomizePitch: true);

                if (patronDisparo == TipoDisparo.HaciaAdelante) DisparoAdelante();
                else if (patronDisparo == TipoDisparo.Estrella) DisparoEstrella();
                else if (patronDisparo == TipoDisparo.EnX) DisparoEnX();
            }
        }

        void DisparoAdelante()
        {
            GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            ExInBullet bulletScript = newBullet.GetComponent<ExInBullet>();

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

            Vector2[] direcciones = {
                new Vector2(1, 1).normalized,
                new Vector2(-1, 1).normalized,
                new Vector2(1, -1).normalized,
                new Vector2(-1, -1).normalized
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

                ExInPlayerControl playerScript = collision.gameObject.GetComponent<ExInPlayerControl>();
                if (playerScript != null)
                {

                    playerScript.GetDamaged(damageAlChocar);
                }

                Destroy(this.gameObject);
            }
        }
        

        public void TakeDamage(int damage)
        {
            lives -= damage; 
            
            if (lives <= 0)
            {

                Destroy(gameObject);
                return;
            }

            RecibirInercia(Vector2.right * knockbackFuerza);

            if (spriteRenderer != null)
            {
                StartCoroutine(RedFlashEffect());
            }
        }

        IEnumerator RedFlashEffect()
        {
            if (spriteRenderer != null) 
            {
                spriteRenderer.color = Color.red;
                yield return new WaitForSeconds(0.1f);
                spriteRenderer.color = Color.white;
            }
        }

        void OnApplicationQuit()
        {
            seEstaCerrandoElJuego = true;
        }

        void OnDestroy()
        {
            if (seEstaCerrandoElJuego || !gameObject.scene.isLoaded) 
                return;

            if (seDivideAlMorir && enemigoHijoPrefab != null)
            {
                for (int i = 0; i < cantidadHijos; i++)
                {

                    float direccionY = (i % 2 == 0) ? 1f : -1f; 
                    float direccionX = Random.Range(0f, 1f); 

                    Vector2 direccionCalculada = new Vector2(direccionX, direccionY).normalized;                   

                    Vector2 spawnPos = (Vector2)transform.position + (direccionCalculada * 0.5f);

                    GameObject nuevoHijo = Instantiate(enemigoHijoPrefab, spawnPos, Quaternion.identity);

                    ExInEnemy scriptHijo = nuevoHijo.GetComponent<ExInEnemy>();
                    if (scriptHijo != null)
                    {

                        scriptHijo.RecibirInercia(direccionCalculada * fuerzaExplosionHijos);
                    }
                }
            }
        }
    }
}
