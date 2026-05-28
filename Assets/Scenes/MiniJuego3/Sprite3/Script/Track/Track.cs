using UnityEngine;

public class Track : MonoBehaviour
{
    private Collider2D[] _walls;

    private void Awake()
    {
        _walls = GetComponents<Collider2D>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        var bounce = other.GetComponent<MovimientoNave>();
        if (bounce== null) return;
        Vector2 navePosition = other.transform.position;
        Vector2 nearestPoint = navePosition;
        float nearestDist = float.MaxValue;

        foreach (var wall in _walls)
        {
            Vector2 checkPoint = wall.ClosestPoint(navePosition);
            float distance = Vector2.Distance(navePosition, checkPoint);
            if (distance < nearestDist)
            {
                nearestDist = distance;
                nearestPoint = checkPoint;
            }


        }
        Vector2 dir = (navePosition - nearestPoint).normalized;
        //Debug.DrawLine(navePosition, navePosition + dir, Color.red, 2.0f);
        //Debug.Log("OnTriggerEnter2D");
        bounce.HitBoundary(dir);
    }
}
