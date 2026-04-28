using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int damage = 5;
    [SerializeField]
    private float speed = 1.5f;

    [SerializeField]
    private EnemyData data;
    private GameObject player;

    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        SetEnemy();
    }

    private void Swarm()
    {
        if (player == null)
        {
            return;
        }

        transform.position=Vector2.MoveTowards(transform.position, player.transform.position, speed*Time.deltaTime);
    }
    // Update is called once per frame
    void Update()
    {
        Swarm();
    }
    private void SetEnemy(){
        GetComponent<Health>().SetHealth(data.HP);
        damage=data.damage;
        speed=data.speed;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            if(collider.GetComponent<Health>() != null)
            {
                collider.GetComponent<Health>().Damage(damage);
                this.GetComponent<Health>().Damage(1000);
            }
        } 
    }
}
