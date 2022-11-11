using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.SceneManagement;

public class ConsoleManager : MonoBehaviour
{
    //Mettre les nouvelles commandes ici, puis dans le Awake en initialisant et ajoutant à la liste, en cas de nouveau type (ex nouvel argument) changer la méthode "HandleInput"
    public static DebugCommand EXAMPLE_COMMAND;
    public static DebugCommand GET_JOIN_CODE;
    public static DebugCommand CREATE_GAME;
    

    //Variables nécessaires à la console
    [SerializeField] private int line_height;

    private float console_height = Screen.height * 1/3;
    private float console_width = Screen.width * 2/3;

    private float text_field_height;
    private bool showConsole;
    private string input;
    private List<string> output;
    private List<object> commandList;
    
    private void Awake()
    {
        text_field_height = line_height + 2;
        output = new List<string>();

        //Ajouter les commandes ici
        EXAMPLE_COMMAND = new DebugCommand("ex", "Example de commande console", "exe", () =>
        {
            TestExample();
        });
        GET_JOIN_CODE = new DebugCommand("get_code", "Demande le JoinCode généré si le client est actuellement hôte.", "get_code", () =>
        {
            GetJoinCode();
        });
        CREATE_GAME = new DebugCommand("host_game", "Créer une nouvelle partie en tant qu'hôte", "host_game", () =>
        {
            CreateGame();
        });

        //Ajouter les commandes à cette liste
        commandList = new List<object>
        {
            EXAMPLE_COMMAND,
            GET_JOIN_CODE,
            CREATE_GAME
        };
    }

    private void OnEnable() {
        Application.logMessageReceived += HandleConsoleMessage;
    }

    public void OnToggleConsole(InputValue value){
        showConsole = !showConsole;
    }

    public void OnReturn(InputValue value)
    {
        if(showConsole)
        {
            HandleInput();
            input = "";
        }
    }

    private void OnGUI() 
    {
        if(showConsole)
        {
            Rect output_view = new Rect(0, 0, console_width, console_height); //La console d'affichage
            GUI.Box(output_view, ""); 
            input = GUI.TextField(new Rect(0, console_height + 1, console_width, text_field_height), input); // la partie texte


            int max_lines_to_display = Mathf.FloorToInt(output_view.height / line_height);
            if(output.Count > max_lines_to_display)
            {
                output.RemoveRange(0,output.Count - max_lines_to_display);
            }
            for(int i=0; i < output.Count;i++){
                Rect text_rect = new Rect(5, line_height * i, console_width - 10, line_height);
                GUI.Label(text_rect,output[i]);
            }
        }
    }

    private void HandleConsoleMessage(string logString, string stackTrace, LogType type)
    {
        output.Add(logString);
    }

    private void HandleInput()
    {
        foreach(object obj in commandList){
            if(obj as DebugCommand != null)
            {
                DebugCommand com = (DebugCommand)obj;
                if(com.CommandId == input)
                    com.Invoke();
            }
            if(obj as DebugCommand<int> != null)
            {
                DebugCommand<int> com = (DebugCommand<int>)obj;
                if(input.Contains(com.CommandId + ' '))
                {
                    try
                    {
                        int value = int.Parse(input.Replace(com.CommandId + ' ', ""));
                        com.Invoke(value);
                    }catch(FormatException){
                        print(string.Format("Paramètre invalide pour la commande '{0}'", com.CommandId));
                    }
                }
            }
        }
    }

    //Méthodes correspondant aux commandes
    private void TestExample()
    {
        print("Example de commande");
    }

    private void GetJoinCode()
    {
        DisplayJoinCode(NetworkClient.join_code);
    }

    private void DisplayJoinCode(string joinCode){
        if(joinCode == ""){
            print("Aucun code disponible, aucune partie n'est en cours");
        }else{
            print("Le Join Code est: " + joinCode);
        }
    }

    public async void CreateGame()
    {        
        //Authentifie le joueur
        await NetworkClient.AuthenticatingAPlayer();
        //Créer une partie en temps qu'hôte, en générant l'allocation qui donne le joinCode
        StartCoroutine(NetworkClient.ConfigureTransportAndStartNgoAsHost(DisplayJoinCode));

        //Destroy(gameObject);
    }


}
