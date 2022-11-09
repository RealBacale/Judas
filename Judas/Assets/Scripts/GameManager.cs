using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] TMP_Text joinCodeDisplay;
    private void Awake() 
    {
        joinCodeDisplay.text = NetworkClient.join_code;
        NetworkClient.OnJoinCodeChange += UpdateJoinCodeDisplay;
    }

    private void UpdateJoinCodeDisplay()
    {
        joinCodeDisplay.text = NetworkClient.join_code;
    }
}
