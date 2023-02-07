using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Entity : NetworkBehaviour
{
    private int _healthPoints;
    public int healthPoints{
        get{return _healthPoints;}
        set{
            _healthPoints = value;
            if(_healthPoints <= 0)
                OnDeath();
        }
    }

    public virtual void OnDeath()
    {
        DestroyEntityServerRpc();
    }

    [ServerRpc]
    private void DestroyEntityServerRpc()
    {
        gameObject.GetComponent<NetworkObject>().Despawn();
    }

}
