using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUI : MonoBehaviour
{

    [SerializeField] private Button btHost;
    [SerializeField] private Button btClient;

    private void Awake() {
        btHost.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
        });
        btClient.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
        });
    }
}
