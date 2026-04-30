using Unity.VisualScripting;
using UnityEngine;

public class HealObject : MonoBehaviour
{
    [SerializeField] private int healAmount = 25;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Health health = collision.gameObject.GetComponent<Health>();
            if(health != null)
            {
                health.Heal(healAmount);
                Destroy(gameObject);
            }
        }
    }
}
