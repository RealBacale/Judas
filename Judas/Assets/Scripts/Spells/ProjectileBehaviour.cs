using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class ProjectileBehaviour : NetworkBehaviour
{
    public int damage;

    public string sourceID;

    void OnTriggerEnter2D(Collider2D other) {
        //Si la cible est un joueur ou un mob
		if (other.tag == "Player" || other.tag == "Mob") {
            HitTarget(other.gameObject);
		}
	}

    private void HitTarget(GameObject target)
    {
        //on trouve l'entité touchée et on lui enlève autant de vie que de degat sur le proj
        Entity ent = target.GetComponent<Entity>();
        ent.healthPoints -= damage;
        //après avoir touché, le proj est détruit (on utilise despawn pour le détruire sur le serveur)
        gameObject.GetComponent<NetworkObject>().Despawn();
    }
}
