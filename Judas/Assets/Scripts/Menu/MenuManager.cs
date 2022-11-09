using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{

    [SerializeField] Button joinButton;
    [SerializeField] TMP_InputField inpFieldCode;

    private void Awake() 
    {
        joinButton.onClick.AddListener(() => {
            JoinGame(inpFieldCode.text);
        });
    }

    private void Start() {
        //On empêche la destruction de ce go pour pouvoir exécuter du code pendant la transition de scènes, on le détruit manuellement après
        DontDestroyOnLoad(gameObject);
    }

    public async void CreateGame()
    {        
        // //Charge la scène de jeu
        SceneManager.LoadScene(1);
        //Authentifie le joueur
        await NetworkClient.AuthenticatingAPlayer();
        //Créer une partie en temps qu'hôte, en générant l'allocation qui donne le joinCode
        StartCoroutine(NetworkClient.ConfigureTransportAndStartNgoAsHost(getJoinCode));

        //Destroy(gameObject);
    }

    public async void JoinGame(string joinCode)
    {
        print("CODE trouvé :" + joinCode);
        if(joinCode.Length == 6)
        {
            //Charge la scène de jeu
            SceneManager.LoadScene(1);
            //Authentifie le joueur
            await NetworkClient.AuthenticatingAPlayer();
            //Rejoin une allocation donnée et configure le zbeul, puis lance en temps que client
            StartCoroutine(NetworkClient.ConfigureTransportAndStartNgoAsConnectingPlayer(joinCode));
           
            //Destroy(gameObject);
        }
    }

    private void getJoinCode(string code)
    {
        NetworkClient.join_code = code;
    }
}
