using UnityEngine;
using System.Collections.Generic;

namespace TopDown.Shooting
{

public class WeaponModifier : MonoBehaviour
{
    [SerializeField] private WordInputPanel wordInputPanel;//referencia al panel de input de palabras
    [SerializeField] private GunController gunController;//referecia al controlador de armas para modificar balas

    [SerializeField] private BulletModifier[] modifiers;//array de modificadores de bala, cada uno con un keyword

    private void Awake()
    {
        if (wordInputPanel != null)
        {
            wordInputPanel.onWordSubmitted.AddListener(HandleWordSubmitted);
        }
    }

    private void OnDestroy()
    {
        if (wordInputPanel != null)
        {
            wordInputPanel.onWordSubmitted.RemoveListener(HandleWordSubmitted);
        }
    }

    private void HandleWordSubmitted(string submittedWord)
    {
        if (string.IsNullOrWhiteSpace(submittedWord) || gunController == null || modifiers == null)
        {
            return;
        }

        string normalizedWord = submittedWord.Trim().ToLowerInvariant();

        foreach (BulletModifier modifier in modifiers)
        {
            if (modifier == null || string.IsNullOrWhiteSpace(modifier.keyword))
            {
                continue;
            }

            if (normalizedWord == modifier.keyword.Trim().ToLowerInvariant())
            {
                gunController.ApplyBulletUpgrade(modifier);
                if (wordInputPanel != null)
                {
                    wordInputPanel.HidePanel();
                }
                return;
            }
        }
    }


    
}
//clase para definir los modificadores de bala, con un keyword para activarlos
[System.Serializable]
public class BulletModifier
{
    public string keyword;

    public int damage = 5;

    public float speed = 10f;

    public float cooldown = 0.4f;
}
}
