using System.Collections;
using UnityEngine;
namespace TopDown.Enemy{//namespace to organize code and avoid naming conflicts
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int health = 100;
    public bool isDead = false;
    private SpriteRenderer spriteRenderer;//sets sprite renderer
    private Color ogColor;//sets og Color 
    private Vector3 ogScale;//sets og scale to return to after size pulse effect

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //tamaño original es el tamaño al iniciar
        ogScale = transform.localScale;
        //si no tiene sprite renderer, 
        // no se puede cambiar el color, asi que 
        // no se asigna el color original
        if (spriteRenderer != null)
        {
            ogColor = spriteRenderer.color;
        }
    }

    void Update()
    {//check every frame if health is 0 or less,
    //and if im not dead yet
        if (health <= 0)
        {
            Die();
        }
    }

    public int getHealth()
    {//just fetch health
        return health;
    }
    public void Damage(int amount)
    {//reduce health by amount 
        health = health - amount;
        StopAllCoroutines();//stop all coroutines to prevent 
        //multiple damage indicators from overlapping
        StartCoroutine(ScalePulse());//start size pulse effect
        StartCoroutine(VisualIndicator(Color.red));//to color red
    }

    private IEnumerator ScalePulse()
    {
        //shrink
        transform.localScale =ogScale * 0.7f;
        //wait like 5 milliseconds
        yield return new WaitForSeconds(0.05f);

        //grow
        transform.localScale =ogScale * 1.2f;
        //wait like 5 milliseconds
        yield return new WaitForSeconds(0.05f);

        //back to normal
        transform.localScale =ogScale;
    }

    private IEnumerator VisualIndicator(Color color)
    {//change color to given color, wait a bit, 
    //then change back to original color
        spriteRenderer.color = color;
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.color = ogColor;
    }

    private void Die()
    {
        isDead = true;//if dead...

        if (ScoreManager.instance != null)
        {//tell score manager to increase score based on enemy type
            ScoreManager.instance.EnemyKilled(gameObject.tag);
        }
        
        // Notify spawner that an enemy died
        Spawner spawner = FindAnyObjectByType<Spawner>();
        if (spawner != null)
        {
            spawner.EnemyDied();
        }
        
        Destroy(gameObject);//then destroy enemy
    }

    /*THIS ISNT USED, WAS FOR DEBUGGING
    public void SetHealth(int health_)
    {//sett health
        this.health = health_;
    }*/
}}