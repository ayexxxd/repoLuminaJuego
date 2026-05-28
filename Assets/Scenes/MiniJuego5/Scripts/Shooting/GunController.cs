using TopDown.Shooting;
using System.Collections;
using UnityEngine;

namespace TopDown.Shooting
{
    public class GunController : MonoBehaviour
    {
        private float cooldownTimer;

        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform firepoint;
        [SerializeField] private Animator muzzleFlashAnimator;
        [SerializeField] private Animator characterAnimator;

        //stats del bullet
        [SerializeField] private int baseDmg = 20;
        [SerializeField] private float baseSpd = 20f;
        [SerializeField] private float baseCd = 0.1f;
        private int currentDmg;
        private float currentSpd;
        private float currentCd;

        [SerializeField] private GameObject objectToHide;//esconder muzzle flash

        [SerializeField] private AudioClip shootSFX;
        private AudioSource audioSource;
        private bool isShooting;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            ResetBulletStats();//iniciar stats del bullet a sus valores base
        }

        private void Update()
        {
            cooldownTimer += Time.deltaTime;//aumentar timer de cooldow
            //removes muzzle flash and shooting animation when not shooting 
            objectToHide.SetActive(isShooting);
        }

        private void OnShoot()
        {//if left click, and not on cooldown
            if (cooldownTimer < currentCd) return;
            //spawn bullet and shoot it
            GameObject bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation, null);
            //bullet spawned at firepoint position and rotation
            Projectile projectile = bullet.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.SetDamage(currentDmg);
                projectile.ShootBullet(firepoint, currentSpd);//funcion para disparar bullet
            }
                //anima personaje y muzzle flash
                characterAnimator.SetBool("shoot", true);
                muzzleFlashAnimator.SetTrigger("shot");
                //play sonido de disparo
                audioSource.PlayOneShot(shootSFX);
            isShooting = true;
            StartCoroutine(ResetShooting());
            cooldownTimer = 0;//set cooldown to 0, so player has to wait for it to reach cooldown time again before shooting again
        }

        private IEnumerator ResetShooting()
        {//esperar un momento antes de resetear animacion
            yield return new WaitForSeconds(0.25f);

            isShooting = false;
     
            characterAnimator.SetBool("shoot", false);
        }

        public void SetBulletStats(int damage, float speed, float cooldown)
        {
            currentDmg = damage;
            currentSpd = speed;
            currentCd = cooldown;
        }

        public void ApplyBulletUpgrade(BulletModifier modifier)
        {
            if (modifier == null)
            {
                return;
            }

            SetBulletStats(modifier.damage, modifier.speed, modifier.cooldown);
        }

        public void ApplyBulletUpgrade(int damage, float speed, float cooldown)
        {
            SetBulletStats(damage, speed, cooldown);
        }

        public void ResetBulletStats()
        {
            currentDmg = baseDmg;
            currentSpd = baseSpd;
            currentCd = baseCd;
        }
    }
}