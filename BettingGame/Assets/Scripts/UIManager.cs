using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button _btnHost;
    [SerializeField] private Button _btnClient;

    // DEBUG NETWORKING -----------------------------------------
    [SerializeField] private TextMeshProUGUI _txtNetworkDebugger;


    private void Awake()
    {
        _btnHost.onClick.AddListener(StartHost);
        _btnClient.onClick.AddListener(StartClient);

        // DEBUG NETWORKING ------------------------------------------
        Application.logMessageReceived += Application_logMessageReceived;
    }

    // DEBUG NETWORKING ------------------------------------------
    private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
    {
        _txtNetworkDebugger.text += condition + Environment.NewLine;
    }
    // ---------------------------------------------------

    private void OnDestroy()
    {
        _btnHost.onClick.RemoveAllListeners();
        _btnClient.onClick.RemoveAllListeners();
    }

    private void StartHost() => NetworkManager.Singleton.StartHost();

    private void StartClient() => NetworkManager.Singleton.StartClient();

}
