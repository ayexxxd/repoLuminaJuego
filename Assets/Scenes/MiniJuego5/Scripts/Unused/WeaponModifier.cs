using UnityEngine;
using System.Collections.Generic;

namespace TopDown.Shooting{//namespace to organize code and avoid naming conflicts
using TopDown.UI;//namespace to organize code and avoid naming conflicts

public class WeaponModifier : MonoBehaviour
{
    [SerializeField] private WordInputPanel wordInputPanel;//referencia al panel de input de palabras
    [SerializeField] private GunController gunController;//referecia al controlador de armas para modificar balas

    [SerializeField] private BulletModifier[] modifiers;//array de modificadores de bala, cada uno con un keyword



    
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
