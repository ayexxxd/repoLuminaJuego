using UnityEngine;
using System.Collections; // Required for Coroutine

public class Enemy : MonoBehaviour
{
    [SerializeField] private int damage = 5;
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private EnemyData data;
    
    private GameObject player;
    private Rigidbody2D body;
    private Coroutine damageCoroutine; // 1. ADDED: Variable to track the damage loop

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        SetEnemy();
    }

    private void Swarm()
    {
        body.MovePosition(Vector2.MoveTowards(body.position, player.transform.position, speed * Time.fixedDeltaTime));
    }

    void FixedUpdate()
    {
        Swarm();
    }

    private void SetEnemy()
    {
        GetComponent<EnemyHealth>().SetHealth(data.HP);
        damage = data.damage;
        speed = data.speed;
    }

    // 2. MODIFIED: Start the repeating damage
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Health health = collider.GetComponent<Health>();
            if (health != null)
            {
                damageCoroutine = StartCoroutine(DealDamageRepeatedly(health));
            }
        } 
    }

    // 3. ADDED: Stop the damage when player leaves
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (damageCoroutine != null) StopCoroutine(damageCoroutine);
        }
    }

    // 4. ADDED: The loop that actually deals the damage
    private IEnumerator DealDamageRepeatedly(Health health)
    {
        while (true)
        {
            health.Damage(damage);
            yield return new WaitForSeconds(1.2f);//deal dmg every 1.2s
        }
    }
}