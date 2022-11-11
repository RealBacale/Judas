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
                gameObject.GetComponent<NetworkObject>().Despawn();
        }
    }


}
