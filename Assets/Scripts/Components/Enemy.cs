using UnityEngine;
using System.Collections;
using MazeAlgorithms;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int health = 100;

    [SerializeField]
    private float moveTime = 1.0f;

    private Cell currentCell;

    public void Init(Cell cell)
    {
        currentCell = cell;
    }

    private void Start()
    {
        StartCoroutine(SwitchingDestinations());
    }

    private void OnTriggerEnter(Collider coll)
    {
        Player player = coll.gameObject.GetComponent<Player>();
        if(player != null)
        {
            player.Hit();
        }
    }

    private Cell GetForwardCell()
    {
        Vector3 forward = transform.forward;
        if(forward == Vector3.forward)
        {
            return currentCell.north;
        }
        if(forward == Vector3.back)
        {
            return currentCell.south;
        }
        if(forward == Vector3.right)
        {
            return currentCell.east;
        }
        if(forward == Vector3.left)
        {
            return currentCell.west;
        }
        return null;
    }

    private IEnumerator SwitchingDestinations()
    {
        while(true)
        {
            Cell nextCell = GetForwardCell();
            if(!currentCell.IsLinked(nextCell))
            {
                nextCell = currentCell.RandomLink;
            }
            yield return StartCoroutine(MoveTo(new Vector3(nextCell.x, transform.position.y, nextCell.z)));
            currentCell = nextCell;
        }
    }

    private IEnumerator MoveTo(Vector3 destination)
    {
        Vector3 startPosition = transform.position;
        transform.LookAt(destination);
        float time = 0.0f;
        while(time < moveTime)
        {
            transform.position = Vector3.Lerp(startPosition, destination, (time/moveTime));
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }
        transform.position = destination;
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
        GameObject psGo = Instantiate(Resources.Load<GameObject>(Paths.Particles.ENEMY_DIE));
        psGo.transform.position = transform.position;
        ParticleSystem ps = psGo.GetComponent<ParticleSystem>();
        ps.Play();

        Destroy(psGo, ps.startLifetime);
        Destroy(gameObject);
    }
}
