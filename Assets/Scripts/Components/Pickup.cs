using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider coll)
    {
        Player player = coll.GetComponent<Player>();
        if(player != null)
        {
            player.CollectPickup(this);
            Destroy(gameObject);
        }
    }
}
