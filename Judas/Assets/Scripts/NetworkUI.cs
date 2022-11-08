using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NetworkUI : MonoBehaviour
{

    [SerializeField] private Button btHost;
    [SerializeField] private Button btClient;
    [SerializeField] private TMP_InputField inJoinCode;
    [SerializeField] private GameObject clientManager;
    private NetworkClient client;
    private void Start() {
        client = clientManager.GetComponent<NetworkClient>();
        btHost.onClick.AddListener(() => {
            Host();
        });
        btClient.onClick.AddListener(() => {
            Join();
        });
    }


    private void Host()
    {
        StartCoroutine(client.ConfigureTransportAndStartNgoAsHost());
    }

    private void Join() 
    {
        if(inJoinCode.text != "")
        {
            StartCoroutine(client.ConfigureTransportAndStartNgoAsConnectingPlayer(inJoinCode.text));    
        }
    }
}
