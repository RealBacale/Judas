using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    public string PlayerID;
    //Le sort de base actuellement équipé par le joueur
    public Spell currentSpell;
    [SerializeField] private float playerSpeed;
    [SerializeField] private GameObject projectile;

    [SerializeField] private float defaultAttackSpeed;
    [SerializeField] private float defaultProjectileSpeed;
    [SerializeField] private int defaultDamage;
    [SerializeField] private int defaultHP;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 lookDirection = Vector3.zero;

    private Vector2? mousePos = null;

    private bool isOnCoolDown = false;
    private bool isFiring = false;

    private PlayerControls controls;

    private void Awake() {
        controls = new PlayerControls();
        base.healthPoints = defaultHP;
    }

    public override void OnNetworkSpawn()
    {
        PlayerID = OwnerClientId.ToString();
    }

    private void OnEnable() {
        controls.Player.Fire.started += StartFiring;
        controls.Player.Fire.canceled += StopFiring;
        controls.Player.Fire.Enable();
    }


    private void OnDisable() {
        controls.Player.Fire.started -= StartFiring;
        controls.Player.Fire.canceled -= StopFiring;
    }

    private void Start() {
        //On créé un sort de base pour tous les joueurs 
        currentSpell = new Spell(defaultAttackSpeed, defaultProjectileSpeed, defaultDamage);

        //Si on est sur le joueur du client actuel, on lu iattache la caméra principale
        if(IsOwner)
        {
            CameraBehaviour cam = Camera.main.GetComponent<CameraBehaviour>();
            if(cam != null)
                cam.target = this.transform;
        }
    }

    

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate() 
    {
        //DEBUG
        Spell check = new Spell(defaultAttackSpeed, defaultProjectileSpeed, defaultDamage);
        if (currentSpell != check)
            currentSpell = check;

        //Si le client actuel possède l'objet, permet de ne déplacer que le bon joueur
        if(IsOwner)
        {
            //On applique directement le vecteur à la position du joueur, avec l'intervalle de temps et la vitesse
            transform.position += moveDirection * playerSpeed * Time.deltaTime;

            //Pour la rotation, si souris on force le joueur à regarder dans sa direction, sinon on applique une roation basée sur l'input du joystick
            if(mousePos != null)//Uniquement pour souris
            {
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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

    // private void OnFire(InputValue value)
    // {
    //     if(IsOwner)
    //     {
    //         CreateProjServerRpc();
    //     }
    // }

    private void StartFiring(InputAction.CallbackContext obj)
    {
        if(IsOwner){
            isFiring = true;
            StartCoroutine(Fire());
        }
    }

    private void StopFiring(InputAction.CallbackContext obj)
    {
        if(IsOwner)
            isFiring = false;
    }

    private IEnumerator Fire() 
    {
        CreateProjServerRpc();
        yield return null;
        if(isFiring)
            StartCoroutine(Fire());
    }

    private IEnumerator CoolDown(float cdTime){
        yield return new WaitForSecondsRealtime(cdTime);
        isOnCoolDown = false;
    }

    [ServerRpc]
    private void CreateProjServerRpc(){
        //si le sort n'est pas en CD
        if(!isOnCoolDown){
            //On instantie le projectile
            GameObject proj = Instantiate(projectile, transform.position, transform.rotation); 
            //On récupère le composant proj pour ajouter les degats et l'id du tireur
            ProjectileBehaviour projScript = proj.GetComponent<ProjectileBehaviour>();
            projScript.damage = currentSpell.damage;
            projScript.sourceID = PlayerID;

            //On le fait spawn pour indiquer au serveur qu'il faut l'instancier chez tous les clients
            proj.GetComponent<NetworkObject>().Spawn(true);
            //on le met en cd  
            isOnCoolDown = true;
            // on lance un timer pour mettre fin au cd, 1/la vitesse d'attaque
            StartCoroutine(CoolDown(1 / currentSpell.attackSpeed));
            //puis on ajoute une force au proj pour qu'il parte tout droit, on tient compte de sa vitesse
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            rb.AddForce(proj.transform.up * currentSpell.projectileSpeed, ForceMode2D.Impulse);


        }
    }
}
