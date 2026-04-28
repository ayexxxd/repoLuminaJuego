using UnityEngine;

[CreateAssetMenu(fileName = "Data",menuName ="ScriptableObjects/Enemy", order = -1)]
public class EnemyData : ScriptableObject
{
   [SerializeField]
   public int HP;
   [SerializeField]
   public int damage;
   [SerializeField]
   public float speed;
}
