using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform player;
    private float width;
    private float height;

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        width = sr.bounds.size.x;
        height = sr.bounds.size.y;
    }

    void Update()
    {
        Vector3 pos = transform.position;
        Vector3 playerPos = player.position;

        // Horizontal reposition
        if (playerPos.x > pos.x + width)
        {
            transform.position = new Vector3(pos.x + width * 2, pos.y, pos.z);
        }
        else if (playerPos.x < pos.x - width)
        {
            transform.position = new Vector3(pos.x - width * 2, pos.y, pos.z);
        }

        // Vertical reposition
        if (playerPos.y > pos.y + height)
        {
            transform.position = new Vector3(transform.position.x, pos.y + height * 2, pos.z);
        }
        else if (playerPos.y < pos.y - height)
        {
            transform.position = new Vector3(transform.position.x, pos.y - height * 2, pos.z);
        }
    }
}