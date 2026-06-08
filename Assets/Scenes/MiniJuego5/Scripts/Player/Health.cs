using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{

    [SerializeField] private int health = 100;//vida actual
    [SerializeField] private int healthmax = 100;//vida maxima`
    private bool isDead;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    [SerializeField] private AudioClip startSFX;
    [SerializeField] private AudioClip healSFX;
    private AudioSource audioSource;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // startSFX removed to avoid overlapping with looped background music
    }


    void Update()
    {
        if (isDead)
        {
            return;
        }

        if (Keyboard.current != null && Keyboard.current.kKey.wasPressedThisFrame)
        {
            Damage(10);
        }
        if (Keyboard.current != null && Keyboard.current.hKey.wasPressedThisFrame)
        {
            Heal(10);
        }
        if(health <= 0)
        {
            Die();
        }
    }
    private IEnumerator VisualIndicator(Color color)
    {//cambiar color a color de daño o curacion y regresar a color original
        this.spriteRenderer.color = color;
        yield return new WaitForSeconds(0.15f);
        this.spriteRenderer.color = originalColor;
    }
    public int getHealth(){
        return health;
    }
    public void Damage(int amount)
    {//daño a jugador
        health -= amount;
        StartCoroutine(VisualIndicator(Color.red));
    }

    public void Heal(int amount)
    {
        StartCoroutine(VisualIndicator(Color.green));   
        audioSource.PlayOneShot(healSFX);
        //si vida actual mas curacion es mayor a vida maxima, setear vida a maxima
        if(health + amount > healthmax)
        {
           health =healthmax ;
        }
        else
        {
             health = health + amount;
        }
    }
    private void Die()
    {
        if (isDead)
        {
            return;
        }
        isDead = true;
        SceneManager.LoadScene("ShooterEnd");
    }
    }

