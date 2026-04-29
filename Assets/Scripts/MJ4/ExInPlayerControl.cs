using UnityEngine;
using UnityEngine.InputSystem;

namespace DefensoresDeSoftware 
{
    public class ExInPlayerControl : MonoBehaviour
    {
        [Header("Movimiento (Teclado)")]
        public float moveSpeed = 8f;
        public Rigidbody2D rig;

        [Header("Sistema de Disparo")]
        public GameObject bulletPrefab;
        public Transform firePoint; 
        public float fireRate = 0.25f; 
        public Vector2 fireDirection = Vector2.right; 
        

        private Vector2 moveInput;
        private float nextFireTime = 0f;

        void Update()
        {
            // Variables temporales para registrar la intención de movimiento
            float xInput = 0f;
            float yInput = 0f;
            
            // Leemos qué flechas del teclado está presionando el jugador
            if (Keyboard.current.leftArrowKey.isPressed) xInput = -1f;
            else if (Keyboard.current.rightArrowKey.isPressed) xInput = 1f;

            if (Keyboard.current.downArrowKey.isPressed) yInput = -1f;
            else if (Keyboard.current.upArrowKey.isPressed) yInput = 1f;

            // Recortamos el movimiento en diagonal para que no camine más rápido
            moveInput = new Vector2(xInput, yInput).normalized;

            // Verificamos si presionó espacio y si ya pasó el tiempo de recarga del arma
            if (Keyboard.current.spaceKey.isPressed && Time.time >= nextFireTime)
            {
                Shoot();
                // Registramos en qué momento podrá volver a disparar
                nextFireTime = Time.time + fireRate;
            }
        }

        void FixedUpdate()
        {
            // 1. Calculamos a dónde intentará ir la nave en esta fracción de segundo
            // (Time.fixedDeltaTime asegura que la velocidad sea constante sin importar los FPS)
            Vector2 posicionFutura = rig.position + (moveInput * moveSpeed * Time.fixedDeltaTime);

            // 2. Le aplicamos el muro matemático a esa posición IMAGINARIA, antes de movernos
            posicionFutura.x = Mathf.Clamp(posicionFutura.x, ExInGameControl.Instance.minX, ExInGameControl.Instance.maxX);
            posicionFutura.y = Mathf.Clamp(posicionFutura.y, ExInGameControl.Instance.minY, ExInGameControl.Instance.maxY);

            // 3. Movemos el objeto de forma segura, sin que choque ni vibre
            rig.MovePosition(posicionFutura);
        }

        void Shoot()
        {
            // Instanciamos una copia de la bala en la posición de nuestro "cañón"
            GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            
            // Buscamos el script de la bala para darle la orden de disparo
            ExInBullet bulletScript = newBullet.GetComponent<ExInBullet>();
            if (bulletScript != null)
            {
                bulletScript.Fire(fireDirection);
            }
        }
    }
}