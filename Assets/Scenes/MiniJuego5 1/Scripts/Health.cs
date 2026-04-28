using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public bool isDead = false;
    [SerializeField] private int health = 100;
    [SerializeField] private int MAX_HEALTH = 100;
    [SerializeField] private bool isPlayer = true;

    [Header("Debug Controls")]
    [SerializeField] private int debugDamageAmount = 10;
    [SerializeField] private int debugHealAmount = 10;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.kKey.wasPressedThisFrame)
        {
            Damage(debugDamageAmount);
        }

        if (Keyboard.current != null && Keyboard.current.hKey.wasPressedThisFrame)
        {
            Heal(debugHealAmount);
        }

        if (!isDead && health <= 0)
        {
            Die();
        }
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
    }
    public void Heal(int amount)
    {
        if(amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("cannot work");
        }
        
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

