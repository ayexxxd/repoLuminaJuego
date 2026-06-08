using UnityEngine;

namespace TopDown.Shooting
{
public class HealObject : MonoBehaviour
{
    [SerializeField] private int healAmount = 25;
    void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Health health = collision.GetComponent<Health>();
            health.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
}