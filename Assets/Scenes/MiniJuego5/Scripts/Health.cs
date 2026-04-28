using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public bool isDead = false;
    [SerializeField] private int health = 100;
    [SerializeField] private int MAX_HEALTH = 100;
    [SerializeField] private bool isPlayer = true;
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
        if (Keyboard.current != null && Keyboard.current.kKey.wasPressedThisFrame)
        {
            Damage(10);
        }

        if (Keyboard.current != null && Keyboard.current.hKey.wasPressedThisFrame)
        {
            Heal(10);
        }

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
    public int getMax_Health(){
        return MAX_HEALTH;
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
    public void Heal(int amount)
    {
        if(amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("cannot work");
        }
        this.StartCoroutine(VisualIndicator(Color.green));
        
        if(this.health + amount > MAX_HEALTH)
        {
           this.health =MAX_HEALTH ;
        }
        else
        {
             this.health += amount;
        }
    }
    private void Die()
    {
        isDead = true;
        if(isPlayer)
        {
            SceneManager.LoadScene("EndScene");
            return;
        }
        Destroy(gameObject);
    }

    public void SetHealth(int health_){
        this.health=health_;
    }
    }

