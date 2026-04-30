using UnityEngine;
using System.Collections.Generic;

namespace TopDown.Shooting{//namespace to organize code and avoid naming conflicts

///<summary>
/// WeaponModifier listens to word input and applies bullet attribute changes based on keywords.
/// Maps words like "fire", "ice", "spread" to different bullet stats (damage, speed, etc.).
public class WeaponModifier : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WordInputPanel wordInputPanel;
    [SerializeField] private GunController gunController;

    [Header("Weapon Modifiers")]
    [SerializeField] private BulletModifier[] modifiers;
    private Dictionary<string, BulletModifier> modifierMap;

    private void Awake()
    {
        BuildModifierMap();
    }

    private void OnEnable()
    {
        if (wordInputPanel != null)
        {
            wordInputPanel.onValidWord.AddListener(OnValidWord);
        }
    }

    private void OnDisable()
    {
        if (wordInputPanel != null)
        {
            wordInputPanel.onValidWord.RemoveListener(OnValidWord);
        }
    }

    ///build a dictionary of keywords to modifiers for fast lookup.
    private void BuildModifierMap()
    {
        modifierMap = new Dictionary<string, BulletModifier>(System.StringComparer.OrdinalIgnoreCase);

        if (modifiers != null)
        {
            for (int i = 0; i < modifiers.Length; i++)
            {
                if (!string.IsNullOrEmpty(modifiers[i].keyword))
                {
                    modifierMap[modifiers[i].keyword] = modifiers[i];
                }
            }
        }
    }

    ///called when a valid word is entered. Applies the corresponding bullet modifier.
    public void OnValidWord(string word)
    {
        if (modifierMap.TryGetValue(word, out BulletModifier modifier))
        {
            ApplyModifier(modifier);
            Debug.Log($"Weapon modified: {word} - Damage: {modifier.damage}, Speed: {modifier.projectileSpeed}, Cooldown: {modifier.cooldown}");
        }
    }
    ///applies a bullet modifier to the gun controller.
    private void ApplyModifier(BulletModifier modifier)
    {
        if (gunController != null)
        {
            gunController.SetBulletStats(modifier.damage, modifier.projectileSpeed, modifier.cooldown);
            // Add more modifiers here as needed (spread, size, etc.)
        }
    }

    ///resets the weapon to default stats.
    public void ResetToDefault()
    {
        if (gunController != null)
        {
            gunController.ResetBulletStats();
        }
    }
}

///data class representing a bullet modifier tied to a keyword.
[System.Serializable]
public class BulletModifier
{
    [Tooltip("The keyword that triggers this modifier (e.g., 'fire', 'ice', 'spread')")]
    public string keyword;

    [Tooltip("Damage dealt by the bullet")]
    public int damage = 5;

    [Tooltip("Speed of the projectile")]
    public float projectileSpeed = 10f;

    [Tooltip("Cooldown/fire rate between shots (lower = faster)")]
    public float cooldown = 0.4f;
}
}
