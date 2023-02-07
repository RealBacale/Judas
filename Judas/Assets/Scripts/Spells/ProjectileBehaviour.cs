using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class ProjectileBehaviour : NetworkBehaviour
{
    public int damage;
    //Coordonnée x maximale où le proj est considéré comme hors de la map et est détruit, fonctionne en positif et négatif : [-x;x]
    public int maxX;
    //Coordonnée y maximale où le proj est considéré comme hors de la map et est détruit, fonctionne en positif et négatif : [-y;y]
    public int maxY;

    public string sourceID;

    void FixedUpdate() {
        if(IsServer){
            //si le proj est hors de la map il est détruit
            if(Mathf.Abs(transform.position.x) > maxX || Mathf.Abs(transform.position.y) > maxY)
                DestroyProjServerRpc();
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(IsServer)
        {
            //Si la cible est un joueur 
            if (other.tag == "Player") {
                Player player = other.GetComponent<Player>();
                //On ne le touche que si le joueur n'est pas le tireur, et qu'il est en vie
                if(player.PlayerID != sourceID && player.healthPoints > 0)
                {
                    HitTarget(other.gameObject);
                }
            //Si c'est un mob
            }else if(other.tag == "Mob"){
                HitTarget(other.gameObject);
            }else if(other.tag == "Wall"){
                //si on touche un mur le proj est détruit
                DestroyProjServerRpc();
            }
        }
	}

    private void HitTarget(GameObject target)
    {
        //on trouve l'entité touchée et on lui enlève autant de vie que de degat sur le proj
        Entity ent = target.GetComponent<Entity>();
        ent.healthPoints -= damage;

        //Pour éviter qu'un même proj fasse plusieurs dégâts le temps qu'il soit détruit, on passe les dégâts à zéro avant le rpc
        damage = 0;
        //après avoir touché, le proj est détruit (on utilise despawn pour le détruire sur le serveur)
        DestroyProjServerRpc();
    }

    [ServerRpc]
    private void DestroyProjServerRpc()
    {
        gameObject.GetComponent<NetworkObject>().Despawn();
    }
}
