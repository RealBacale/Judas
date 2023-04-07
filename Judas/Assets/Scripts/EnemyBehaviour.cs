using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : Entity
{
    [SerializeField] private int damage;
    private int defaultHP = 50;

    public override int DefaultHealth 
    {
        get {return defaultHP;}
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(IsServer)
        {
            print("Collisison Enemy with " + other.tag);
            //Si la cible est un joueur 
            if (other.tag == "Player") {
                Player player = other.GetComponent<Player>();
                player.SetHealthServerRpc(player.HealthPoints.Value - damage);
            }
        }
	}
}
