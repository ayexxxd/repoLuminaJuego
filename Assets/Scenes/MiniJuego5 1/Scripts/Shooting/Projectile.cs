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
            /*lifeTimer = 0;
            body.linearVelocity = Vector2.zero;
            transform.position=shootPoint.position;
            transform.rotation=shootPoint.rotation;
            gameObject.SetActive(true);

            body.AddForce(-transform.up * speed, ForceMode2D.Impulse);*/
            lifeTimer = 0;
    
    // Set position and rotation
    transform.position = shootPoint.position;
    transform.rotation = shootPoint.rotation;
    
    // Activate
    gameObject.SetActive(true);

    // Instead of AddForce, set the velocity directly
    // This is instant and consistent
    body.linearVelocity = -transform.up * speed;
        }

    private void Update()
        {
          lifeTimer += Time.deltaTime;
    
    // DEBUG: This will print "Bullet Alive" in the console every frame
    //Debug.Log("Bullet Alive at: " + transform.position); 

    if(lifeTimer >= lifetime)
    {
        //Debug.Log("Bullet Despawning!"); // Check if this prints prematurely
        gameObject.SetActive(false);
    }
        }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Health enemyHealth = collision.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.Damage(damage);
            }
            gameObject.SetActive(false);
        }
    }
}
}