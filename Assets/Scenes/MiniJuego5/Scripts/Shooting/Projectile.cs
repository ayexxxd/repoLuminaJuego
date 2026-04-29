using UnityEngine;

namespace TopDown.Shooting
{
    [RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
   [Header("Movement Stats")]
   [SerializeField]private float speed = 10f;
   [SerializeField]private float lifetime = 5f;
    [Header("Damage")]
    [SerializeField] private int damage = 5;
   private Rigidbody2D body;
   private float lifeTimer;

   private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
        }
    public void ShootBullet(Transform shootPoint)
        {
            lifeTimer = 0;
            transform.position = shootPoint.position;
            transform.rotation = shootPoint.rotation;
            gameObject.SetActive(true);
            body.linearVelocity = -transform.up * speed;
        }

    private void Update()
        {//life timer increases
          lifeTimer += Time.deltaTime;
    //Debug.Log("Bullet Alive at: " + transform.position); 

    if(lifeTimer >= lifetime)
    {//if lifetime is higher than lifetime, destroy bullet
        Destroy(gameObject);//completely destroy bullet from memory
    }
        }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealth enemyHealth = collision.GetComponentInParent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.Damage(damage);
            Destroy(gameObject);
        }
    }
}
}