using Unity.VisualScripting;
using UnityEngine;
namespace TopDown.Shooting{//namespace to organize code and avoid naming conflicts

public class HealObject : MonoBehaviour
{
    [SerializeField] private int healAmount = 25;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Health health = collision.gameObject.GetComponent<Health>();
                health.Heal(healAmount);
                Destroy(gameObject);
        }
    }
}
}