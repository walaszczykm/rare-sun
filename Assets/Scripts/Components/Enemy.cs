using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    private const float MIN_DISTANCE_TO_WALL = 1.5f;

    [SerializeField]
    private int health = 100;
    [SerializeField]
    private int moveSpeed = 2;
    [SerializeField]
    private LayerMask wallsLayerMask;

    private void Start()
    {
        StartCoroutine(MoveForwardAndCheckForWall());
    }

    //AI
    private IEnumerator MoveForwardAndCheckForWall()
    {
        while(true)
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, transform.forward);
            if(Physics.Raycast(ray, out hit, MIN_DISTANCE_TO_WALL, wallsLayerMask))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.blue);

                transform.position = hit.point;
                transform.Translate(-transform.forward * MIN_DISTANCE_TO_WALL, Space.World);

                LookForDirectionAndRotate();
                //break;
            }
            else
            {
                transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);
            }

            yield return new WaitForEndOfFrame();
        }
    }
    
    private void LookForDirectionAndRotate()
    {
        List<Ray> rays = new List<Ray>();
        rays.Add(new Ray(transform.position, -transform.right));
        rays.Add(new Ray(transform.position, -transform.forward));
        rays.Add(new Ray(transform.position, transform.right));

        foreach(Ray ray in rays)
        {
            if(!Physics.Raycast(ray, MIN_DISTANCE_TO_WALL + 2.0f, wallsLayerMask))
            {
                transform.Rotate(transform.up, Vector3.Angle(transform.forward, ray.direction));
            }
        }
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
