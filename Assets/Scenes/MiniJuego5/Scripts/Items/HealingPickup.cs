using UnityEngine;

public class HealingPickup : MonoBehaviour
{
    [SerializeField] private int healAmount = 25;
    [SerializeField] private bool destroyOnUse = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Health health = other.GetComponent<Health>();
            if (health != null)
            {
                health.Heal(healAmount);
            }

            if (destroyOnUse)
            {
                Destroy(gameObject);
            }
        }
    }
}
