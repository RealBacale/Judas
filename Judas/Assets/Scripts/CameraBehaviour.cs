using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{

    public Transform target = null;

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            //On déplace la caméra actuelle vers le x et y de la target, mais on garde le z pour ne pas modifier l'angle de vue
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        }
    }
}
