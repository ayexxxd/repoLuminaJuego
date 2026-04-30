using UnityEngine;

public class HealingPickup : MonoBehaviour
{
    [SerializeField] private int healAmount = 25;//cantidad de curacion
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Health health = other.GetComponent<Health>();//obtener componente de salud del jugador
            health.Heal(healAmount);//regenerar vida a jugador
            Destroy(gameObject);//borrar objeto
        }
    }
}
