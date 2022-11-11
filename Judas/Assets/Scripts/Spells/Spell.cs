using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell
{
    private float _attackSpeed;
    private float _projectileSpeed;
    private int _damage;

    public float attackSpeed  {
        get{return _attackSpeed;} 
        set{_attackSpeed = value;}
    }
    public float projectileSpeed  {
        get{return _projectileSpeed;} 
        set{_projectileSpeed = value;}
    }
    public int damage {
        get{return _damage;} 
        set{_damage = value;}
    }

    public Spell(float atSpeed, float projSpeed, int dmg)
    {
        _attackSpeed = atSpeed;
        _projectileSpeed = projSpeed;
        _damage = dmg;
    }
}
