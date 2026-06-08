using UnityEngine;

namespace TopDown.Shooting{
using TopDown.Enemy;//namespace to organize code and avoid naming conflicts

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private int damage = 5;
   private Rigidbody2D body;
   private float lifeTimer;

   private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
        }
    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    public void ShootBullet(Transform shootPoint, float bulletSpd)
        {//reset cooldown, posicion y rotacion, y disparar bala
            lifeTimer = 0;
            transform.position = shootPoint.position;
            transform.rotation = shootPoint.rotation;
            gameObject.SetActive(true);
            body.linearVelocity = -transform.up * bulletSpd;//disparar hacia adelante
        }

    private void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
        EnemyHealth enemyHealth = collision.GetComponentInParent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.Damage(damage);
        }
    }
}
}