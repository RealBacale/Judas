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
                DestroyProj();
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(IsServer)
        {
            //Si la cible est un joueur 
            if (other.tag == "Player") {
                Player player = other.GetComponent<Player>();
                //On ne le touche que si le joueur n'est pas le tireur, et qu'il est en vie
                if(player.PlayerID != sourceID && player.HealthPoints.Value > 0)
                {
                    HitTarget(other.gameObject);
                }
            //Si c'est un mob
            }else if(other.tag == "Mob"){
                HitTarget(other.gameObject);
            }else if(other.tag == "Wall"){
                //si on touche un mur le proj est détruit
                DestroyProj();
            }
        }
	}

    
    private void HitTarget(GameObject target)
    {
        if(IsServer)
        {
            print("Collisison Proj with " + target.tag);
            //on trouve l'entité touchée et on lui enlève autant de vie que de degat sur le proj
            Entity ent = target.GetComponent<Entity>();
            if(ent is null){
                print("ERREUR EntityComponent introuvable");
            }else{
                print("Component Entity : " + ent.name + "___" + ent.ToString());
            }
            print("Appel RPC de PV " + ent.HealthPoints.Value + " ,DMG " + damage);
            //On peut (et on doit) appeler directement la valeur de la variable network car on sait qu'on est actuellement sur le serveur
            ent.HealthPoints.Value = ent.HealthPoints.Value - damage;

            //Pour éviter qu'un même proj fasse plusieurs dégâts le temps qu'il soit détruit, on passe les dégâts à zéro avant le rpc
            damage = 0;
            //après avoir touché, le proj est détruit (on utilise despawn pour le détruire sur le serveur)
            DestroyProj();
        }
    }

    private void DestroyProj()
    {
        gameObject.GetComponent<NetworkObject>().Despawn();
    }
}
