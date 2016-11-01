using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour
{
    private const float ROTATION_SPEED = 100.0f;
    
    public void SetupPickup()
    {
        AddTrigger();
        StartCoroutine(RotateCoroutine());
    }

    private void OnTriggerEnter(Collider coll)
    {
        Player player = coll.GetComponent<Player>();
        if(player != null)
        {
            OnPickedup(player);
            Destroy(gameObject);
        }
    }

    private void AddTrigger()
    {
        BoxCollider coll = gameObject.AddComponent<BoxCollider>();
        coll.size = Vector3.one;
        coll.isTrigger = true;
    }

    private IEnumerator RotateCoroutine()
    {
        while(true)
        {
            transform.Rotate(transform.up, Time.deltaTime * ROTATION_SPEED);
            yield return null;
        }
    }

    protected virtual void OnPickedup(Player player) { }
}
