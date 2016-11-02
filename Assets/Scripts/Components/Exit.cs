using UnityEngine;

public class Exit : MonoBehaviour
{
    private void OnTriggerEnter(Collider coll)
    {
        Player player = coll.GetComponent<Player>();
        if (player != null)
        {
            LevelGenerator.Instance.GenerateLevel();
            player.ResetPlayer();           
        }
    }
}
