using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    private Quaternion fixedRotation;

    // Start is called before the first frame update
    void Start()
    {
        //fixedRotation = transform.rotation;
        fixedRotation = Quaternion.Euler(0,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.rotation != fixedRotation)
            transform.rotation = fixedRotation;
    }
}
