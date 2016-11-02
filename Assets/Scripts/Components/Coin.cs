using UnityEngine;
using System.Collections;

public class Coin : Pickup
{
    [SerializeField]
    private int value;

    protected override void OnPickedup(Player player)
    {
        player.AddPoints(value);
    }
}
