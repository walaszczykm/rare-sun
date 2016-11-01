using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int health = 100;

    private void Awake()
    {
    }

    public void Hit(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        //TODO: die effect
        Destroy(gameObject);
    }
}
