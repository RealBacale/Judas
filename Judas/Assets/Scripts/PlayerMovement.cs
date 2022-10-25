using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float playerSpeed;
    [SerializeField] private InputAction playerControls;

    private Vector3 moveDirection = Vector3.zero;

   // private InputAction playerControls;

    private void Start() {
        //playerControls = GetComponent<InputAction>().;
    }

    private void OnEnable() 
    {
        playerControls.Enable();
    }

    private void OnDisable() 
    {
        playerControls.Disable();
    }

    

    // Update is called once per frame
    void Update()
    {
        //Si le client actuel possède l'objet, permet de ne déplacer que le bon joueur
        if(IsOwner)
            //On lit les valeurs du système d'input qui renvoie un Vecteur2D, la conversion vers le vecteur 3D est implicite et le z reste à zéro
            moveDirection = playerControls.ReadValue<Vector2>();
    }

    private void FixedUpdate() 
    {
        if(IsOwner)
            //On applique directement le vecteur à la position du joueur, avec l'intervalle de temps et la vitesse
            transform.position += moveDirection * playerSpeed * Time.deltaTime;
    }
}
