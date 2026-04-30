using UnityEngine;
using System.Collections;//required for Coroutine

public class Enemy : MonoBehaviour
{
    [SerializeField] private int damage = 5;//sets damage variable
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private EnemyData data;

    //optional array of sprites you can assign in the Inspector. The enemy will pick one at random when spawned.
    [Header("Appearance")]
    [SerializeField] private Sprite[] possibleSprites;
    
    private GameObject player;
    private Rigidbody2D body;
    private Coroutine damageCoroutine;//variable to track the damage loop
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        SetEnemy();
        PickRandomSprite();
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
    {   //set stats from data
        damage = data.damage;
        speed = data.speed;
    }

    // Pick a random sprite from the inspector array; safe if array is empty or null
    private void PickRandomSprite()
    {
        if (possibleSprites != null && possibleSprites.Length > 0 && spriteRenderer != null)
        {
            int idx = Random.Range(0, possibleSprites.Length);
            spriteRenderer.sprite = possibleSprites[idx];
        }
    }

    //start the repeating damage
    private void OnTriggerEnter2D(Collider2D collider)
    {   //if player enters the trigger, start the damage loop
        if (collider.CompareTag("Player"))
        {
            Health health = collider.GetComponent<Health>();
            if (health != null)
            {
                damageCoroutine = StartCoroutine(Attack(health));
            }
        } 
    }

    //stop the damage when player leaves
    private void OnTriggerExit2D(Collider2D collider)
    {   //if player leaves the trigger, stop the damage loop
        if (collider.CompareTag("Player"))
        {
            if (damageCoroutine != null) StopCoroutine(damageCoroutine);
        }
    }

    //the loop that actually deals the damage
    private IEnumerator Attack(Health health)
    {
        while (true)
        {
            health.Damage(damage);
            yield return new WaitForSeconds(1.2f);//deal dmg every 1.2s
        }
    }
}