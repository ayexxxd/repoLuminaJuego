using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    public bool isDead = false;
    [SerializeField] private int health = 100;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }


    void Update()
    {
        if (!isDead && health <= 0)
        {
            Die();
        }
    }
    private IEnumerator VisualIndicator(Color color)
    {
        this.spriteRenderer.color = color;
        yield return new WaitForSeconds(0.15f);
        this.spriteRenderer.color = originalColor;
    }
    public int getHealth(){
        return health;
    }
    public void Damage(int amount)
    {
        if(amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("cannot work");
        }
        this.health -= amount;
        this.StartCoroutine(VisualIndicator(Color.red));
    }
    private void Die()
    {
        isDead = true;
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.EnemyKilled(gameObject.tag);
        }

        Destroy(gameObject);
    }

    public void SetHealth(int health_){
        this.health=health_;
    }
    }

