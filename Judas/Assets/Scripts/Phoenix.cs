using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phoenix : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other) {
        //Si la cible est un joueur et qu'il est mort, on le res
        if (other.tag == "Player") {
            Player player = other.GetComponent<Player>();
            if(player.HealthPoints.Value <= 0)
                player.ExecuteOnAllClientsServerRpc(PlayerClientActionEnum.Resurect);
        }
	}
}
