using UnityEngine;
using System.Collections;//required for Coroutine

namespace TopDown.Enemy{//namespace to organize code and avoid naming conflicts
public class Enemy : MonoBehaviour
{
    [SerializeField] private int damage = 5;//daño base
    [SerializeField] private float speed = 1.5f;//velocidad base
    [SerializeField] private EnemyData data;//referncia a scriptable object con stats base

    //array de posibles sprites
    [SerializeField] private Sprite[] sprites;
    
    private GameObject player;//referencia al jugador para seguirlo
    private Rigidbody2D body;//referencia al rigidbody para moverlo
    private Coroutine damageCoroutine;//corutina para hacer daño cada cierto tiempo
    private SpriteRenderer spriteRend;//referencia al sprite renderer para cambiar apariencia

    void Start()
    {
        body = GetComponent<Rigidbody2D>();//asignar referencia al rigidbody
        spriteRend = GetComponent<SpriteRenderer>();//asignar referencia al sprite renderer
        player = GameObject.FindGameObjectWithTag("Player");//asignar referencia al jugador buscando por su tag
        SetEnemy();
    }

    private void Swarm()
    {//mueve enemigo hacia el jugador cada frame usando MoveTowards
        body.MovePosition(Vector2.MoveTowards(body.position, player.transform.position, speed * Time.fixedDeltaTime));
    }

    void FixedUpdate()
    {//llamar a Swarm cada frame fijo para seguir al jugador
        Swarm();
    }

    private void SetEnemy()
    {   //setea stats de enemigos a los del scriptable object
        damage = data.damage;
        speed = data.speed;
    }

    //inicia el daño
    private void OnTriggerEnter2(Collider2D collider)
    {   //si el jugador entra al rango del enemigo, iniciar el loop de daño
        if (collider.CompareTag("Player"))//compara tags
        {
            Health health = collider.GetComponent<Health>();
            if (health != null)//si el objeto tiene componente de health, iniciar el loop de daño
            {
                damageCoroutine = StartCoroutine(Attack(health));
                //llamamos funcion attack con el parametro de daño
            }
        } 
    }

    //detiene el daño
    private void OnTriggerExit2D(Collider2D collider)
    {   //si jugador slae de rango, detener loop
        if (collider.CompareTag("Player"))
        {
            if (damageCoroutine != null) StopCoroutine(damageCoroutine);
        }
    }

    //loop que hace daño si enemigo toca al jugador
    private IEnumerator Attack(Health health)
    {
        while (true)
        {
            health.Damage(damage);//hacer daño al jugador
            yield return new WaitForSeconds(1.5f);//hacer daño cada 1.5 s
        }
    }
}}