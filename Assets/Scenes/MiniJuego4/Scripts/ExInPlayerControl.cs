using System.Collections;
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
        public AudioClip shootSFX;

        private float baseMoveSpeed;
        private int baseDamage = 1;

        private SpriteRenderer spriteRenderer;
        private Vector2 moveInput;
        private float nextFireTime = 0f;

        [HideInInspector] public int currentDamage = 1;

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            baseMoveSpeed = moveSpeed;
            baseDamage = 1;

            AplicarMejoras();
        }

        public void AplicarMejoras()
        {
            int bonusDamage = PlayerPrefs.GetInt("BonusDamage", 0);
            int bonusSpeed  = PlayerPrefs.GetInt("BonusSpeed", 0);

            currentDamage = baseDamage + bonusDamage;
            moveSpeed     = baseMoveSpeed + (bonusSpeed * 1f);

            Debug.Log($"[Player] Daño actual: {currentDamage} | Velocidad actual: {moveSpeed}");
        }

        void Update()
        {
            float xInput = 0f;
            float yInput = 0f;
            
            if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)  xInput = -1f;
            else if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed) xInput = 1f;

            if (Keyboard.current.downArrowKey.isPressed || Keyboard.current.sKey.isPressed )  yInput = -1f;
            else if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed) yInput = 1f;

            moveInput = new Vector2(xInput, yInput).normalized;

            if (Keyboard.current.spaceKey.isPressed && Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }

        void FixedUpdate()
        {
            Vector2 posicionFutura = rig.position + (moveInput * moveSpeed * Time.fixedDeltaTime);

            posicionFutura.x = Mathf.Clamp(posicionFutura.x, ExInGameControl.Instance.minX, ExInGameControl.Instance.maxX);
            posicionFutura.y = Mathf.Clamp(posicionFutura.y, ExInGameControl.Instance.minY, ExInGameControl.Instance.maxY);

            rig.MovePosition(posicionFutura);
        }
        
        void Shoot()
        {
            //if (SFXManager.Instance != null)
                //SFXManager.Instance.PlaySFX(shootSFX, randomizePitch: true);

            GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            ExInBullet bulletScript = newBullet.GetComponent<ExInBullet>();
            if (bulletScript != null)
            {
                bulletScript.damage = currentDamage;
                bulletScript.Fire(fireDirection);
            }
        }

        public void GetDamaged(int damage)
        {
            if (spriteRenderer != null && gameObject.activeInHierarchy)
            {
                StartCoroutine(RedFlashEffect());
            }

            if (ExInGameControl.Instance != null)
            {
                ExInGameControl.Instance.TakeDamage(damage);
            }
        }

        IEnumerator RedFlashEffect()
        {
            if (spriteRenderer != null) 
            {
                spriteRenderer.color = Color.red;
                yield return new WaitForSeconds(0.1f);
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = Color.white;
                }
            }
        }
    }
}