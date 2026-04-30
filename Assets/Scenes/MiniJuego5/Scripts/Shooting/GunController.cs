using TopDown.Shooting;
using System.Collections;
using UnityEngine;

namespace TopDown.Shooting
{
    public class GunController : MonoBehaviour
    {
        [SerializeField] private float cooldown = 0.4f;
        private float cooldownTimer;

        [Header("References")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform firepoint;
        [SerializeField] private Animator muzzleFlashAnimator;
        [SerializeField] private Animator characterAnimator;

        [Header("Visual")]
        [SerializeField] private GameObject objectToHide;

        [SerializeField] private AudioClip shootSFX;
        private AudioSource audioSource;
        private bool isShooting;
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            cooldownTimer += Time.deltaTime;
            if (objectToHide != null)
            {//removes muzzle flash and shooting animation when not shooting 
                objectToHide.SetActive(isShooting);
            }
        }

        private void OnShoot()
        {//if left click, and not on cooldown
            if (cooldownTimer < cooldown) return;
            //spawn bullet and shoot it
            GameObject bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation, null);
            //bullet spawned at firepoint position and rotation
            bullet.GetComponent<Projectile>().ShootBullet(firepoint);

            if (characterAnimator != null)
            {//animate player
                characterAnimator.SetBool("shoot", true);
            }

            if (muzzleFlashAnimator != null)
            {//animate muzzle
                muzzleFlashAnimator.SetTrigger("shot");
            }

            if (shootSFX != null && audioSource != null)
            {//play sfx
                audioSource.PlayOneShot(shootSFX);
            }
            isShooting = true;
            StartCoroutine(ResetShooting());
            cooldownTimer = 0;//set cooldown to 0, so player has to wait for it to reach cooldown time again before shooting again
        }

        private IEnumerator ResetShooting()
        {//wait for a short time to reset shooting state, allowing for animations and visual effects to play out
            yield return new WaitForSeconds(0.25f);

            isShooting = false;
            if (characterAnimator != null)
            {//reset shooting animation
                characterAnimator.SetBool("shoot", false);
            }
        }
    }
}