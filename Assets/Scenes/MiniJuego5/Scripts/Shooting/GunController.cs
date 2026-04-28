using TopDown.Shooting;
using Unity.VisualScripting;
using UnityEngine;

namespace TopDown.Shooting{
public class GunController : MonoBehaviour
{
    [Header("Cooldown")]
    [SerializeField]private float cooldown = 0.4f;
    private float cooldownTimer;

    [Header("References")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firepoint;
    [SerializeField] private Animator muzzleFlashAnimator;

   //bullet prefab

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
    }
    private void Shoot()
    {
        if(cooldownTimer < cooldown) return;

        GameObject bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation, null);
        bullet.GetComponent<Projectile>().ShootBullet(firepoint);
        muzzleFlashAnimator.SetTrigger("shoot");
        cooldownTimer = 0;
    }

    private void OnShoot()
    {
        Shoot();
    }

}}
