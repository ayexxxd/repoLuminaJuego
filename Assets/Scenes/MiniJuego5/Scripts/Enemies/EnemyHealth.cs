using System.Collections;
using UnityEngine;
namespace TopDown.Enemy{//namespace to organize code and avoid naming conflicts
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int health = 100;
    private bool isDead;
    private SpriteRenderer spriteRen;//referencia al sprite renderer para cambiar color al recibir daño
    private Color ogColor;//guardar color original
    private Vector3 ogScale;//guardar tamaño original

    private void Awake()
    {
        spriteRen = GetComponent<SpriteRenderer>();//
        //tamaño original es el tamaño al iniciar
        ogScale = transform.localScale;
        //color original es el color al iniciar
        ogColor = spriteRen.color;
    }

    void Update()
    {//checar si vida es 0 o menos
        if (health <= 0)
        {
            Die();
        }
    }
    public void Damage(int damage)
    {//reducir vida por cantidad de daño
        health = health - damage;
        StopAllCoroutines();//detiene corrutinas para evitar que se acumulen

        StartCoroutine(Scale());//indicar daño con pulso de tamaño
        StartCoroutine(VisualIndicator(Color.red));//a color rojo
    }

    private IEnumerator Scale()
    {
        //encoger
        transform.localScale =ogScale * 0.7f;
        //esperar como 5 milisegundos
        yield return new WaitForSeconds(0.05f);

        //crecer
        transform.localScale =ogScale * 1.2f;
        //esperar como 5 milisegundos
        yield return new WaitForSeconds(0.05f);

        //volver a tamaño original
        transform.localScale =ogScale;
    }

    private IEnumerator VisualIndicator(Color color)
    {//cambiar color a color de daño 
    //y regresar a color original
        spriteRen.color = color;
        yield return new WaitForSeconds(0.15f);
        spriteRen.color = ogColor;
    }

    private void Die()
    {
            if (isDead)
            {
                return;
            }

            isDead = true;
            //aumentar score al matar enemigo, usando el tag del enemigo para determinar cuantos puntos da
            ScoreManager.instance.EnemyKilled(gameObject.tag);
            Spawner spawner = FindAnyObjectByType<Spawner>();
            if (spawner != null)
            {
                spawner.EnemyDied();
            }
            Destroy(gameObject);//y borrrar enemigo

    
}}}