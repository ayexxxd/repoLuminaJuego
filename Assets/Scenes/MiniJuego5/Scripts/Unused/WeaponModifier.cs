using UnityEngine;

namespace TopDown.Shooting
{
    public class WeaponModifier : MonoBehaviour
    {
        [SerializeField] private GunController gunController;
        [SerializeField] private BulletModifier[] modifiers;

        public void TryApplyUpgrade(string word)
        {
            if (gunController == null)
            {
                gunController = FindAnyObjectByType<GunController>();
            }
            if (gunController == null)
            {
                Debug.LogWarning("WeaponModifier: No GunController found!");
                return;
            }

            string lower = word.ToLowerInvariant();
            foreach (var mod in modifiers)
            {
                if (mod.keyword.ToLowerInvariant() == lower)
                {
                    gunController.SetBulletStats(mod.damage, mod.speed, mod.cooldown);
                    Debug.Log($"WeaponModifier: Upgraded with '{word}' -> DMG:{mod.damage} SPD:{mod.speed} CD:{mod.cooldown}");
                    return;
                }
            }

            Debug.Log($"WeaponModifier: Word '{word}' not found in modifiers.");
        }
    }

    [System.Serializable]
    public class BulletModifier
    {
        public string keyword;
        public int damage = 5;
        public float speed = 10f;
        public float cooldown = 0.4f;
    }
}
