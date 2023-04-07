using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Entity : NetworkBehaviour
{
    private const int DEFAULT_HEALTH = 50;
    public delegate void delegateHealth();
    public delegateHealth healthChanged = null;

    public NetworkVariable<int> HealthPoints = new NetworkVariable<int>();

    public virtual int DefaultHealth 
    {
        get {return DEFAULT_HEALTH;}
    }


    public override void OnNetworkSpawn()
    {
        if(IsServer){
            print("Set HEALTH " + DefaultHealth);
            HealthPoints.Value = DefaultHealth;
        }
            HealthPoints.OnValueChanged += EntityHealthChanged;
    }

    private void EntityHealthChanged(int previous, int current)
    {
        print("CURRENT_HP_" + current);
        if(current <= 0)
            OnDeath();
        if(healthChanged is not null)
            healthChanged();
    }

    public virtual void OnDeath()
    {
        DestroyEntityServerRpc();
    }

    [ServerRpc]
    public void SetHealthServerRpc(int newValue){
        print("SetHealthRPC " + newValue);
        int oldValue = HealthPoints.Value;
        if(newValue != oldValue){
            HealthPoints.Value = newValue;
        }
    }

    [ServerRpc]
    private void DestroyEntityServerRpc()
    {
        gameObject.GetComponent<NetworkObject>().Despawn();
    }

}
