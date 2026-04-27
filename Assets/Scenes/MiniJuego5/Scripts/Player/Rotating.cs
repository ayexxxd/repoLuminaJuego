using Unity.VisualScripting;
using UnityEngine;
namespace TopDown.Movement
{
public class Rotating : MonoBehaviour
{
   protected void LookAt(Vector3 target)
    {
        //Calc angle between transform and target
        float lookAngle = AngleBetweenPointsTwoPoints(transform.position, target);
        //Assign the target rotation
        transform.eulerAngles = new Vector3(0,0,lookAngle);
    }

    private float AngleBetweenPointsTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}
}