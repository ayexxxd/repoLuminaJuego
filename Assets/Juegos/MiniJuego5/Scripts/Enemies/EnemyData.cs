using UnityEngine;

[CreateAssetMenu(fileName = "Data",menuName ="ScriptableObjects/Enemy", order = -1)]
public class EnemyData : ScriptableObject//ScriptableObject para almacenar datos de enemigos y 
// poder editarlos desde el inspector, para tener varios tipos de enemigos
{
   [SerializeField]
   public int HP;
   [SerializeField]
   public int damage;
   [SerializeField]
   public float speed;
}
