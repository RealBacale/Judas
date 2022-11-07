using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class ClientManager : MonoBehaviour
{
    public static DebugCommand EXAMPLE_COMMAND;

    private List<object> commandList;

    [SerializeField] private float console_height;
    [SerializeField] private float text_field_height;
    [SerializeField] private int line_height;

    private bool showConsole;
    private string input;
    private List<string> output;
    
    private void Awake()
    {
        output = new List<string>();

        EXAMPLE_COMMAND = new DebugCommand("ex", "Example de commande console", "exe", () =>
        {
            TestExample();
        });

        commandList = new List<object>
        {
            EXAMPLE_COMMAND
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
            float console_width = 2 * Screen.width / 3;
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

    private void TestExample()
    {
        print("Example de commande");
    }
}
