using UnityEngine;
using System.Collections;

public class FirstAidKit : Pickup
{
    [SerializeField]
    private int hp;

    protected override void OnPickedup(Player player)
    {
        player.AddHealth(hp);
    }
}
