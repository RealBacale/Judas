using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float playerSpeed;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 lookDirection = Vector3.zero;

    private Vector2? mousePos = null;

    private void Start() {
        
    }

    

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate() 
    {
        //Si le client actuel possède l'objet, permet de ne déplacer que le bon joueur
        if(IsOwner)
        {
            //On applique directement le vecteur à la position du joueur, avec l'intervalle de temps et la vitesse
            transform.position += moveDirection * playerSpeed * Time.deltaTime;

            //Pour la rotation, si souris on force le joueur à regarder dans sa direction, sinon on applique une roation basée sur l'input du joystick
            if(mousePos != null)//Uniquement pour souris
            {
                //Pour calculer la rotation on applique au vecteur up de notre perso la différence le pos de la souris et de la pos du perso
                transform.up = (Vector2)mousePos - (new Vector2(transform.position.x, transform.position.y));
            }else{//Uniquement pour les joysticks
                //On applique la rotation sur l'axe z grâce à Euler (à tester)
                transform.rotation = Quaternion.Euler(lookDirection );//* Time.deltaTime);
            }
        }

    }

    private void OnMove(InputValue value)
    {
        //On lit les valeurs du système d'input qui renvoie un Vecteur2D, la conversion vers le vecteur 3D est implicite et le z reste à zéro
        if(IsOwner)
            moveDirection = value.Get<Vector2>();
    }

    private void OnLook(InputValue value)
    {
        if(IsOwner)
        {
            Vector3 inp = value.Get<Vector2>();
            lookDirection = new Vector3(0f,0f, inp.x + inp.y);
            print(inp);
        }
    }

    private void OnMousePos(InputValue value)
    {
        if(IsOwner)
        {
            mousePos = Camera.main.ScreenToWorldPoint(value.Get<Vector2>());
        }
    }
}
