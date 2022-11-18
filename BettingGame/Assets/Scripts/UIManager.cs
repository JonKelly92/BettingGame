using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : NetworkBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private Button _btnHost;
    [SerializeField] private Button _btnClient;

    [SerializeField] private Button _btnGreen;
    [SerializeField] private Button _btnRed;

    [SerializeField] private TextMeshProUGUI _chipCount;
    [SerializeField] private TextMeshProUGUI _timer;

    // DEBUG NETWORKING -----------------------------------------
    [SerializeField] private TextMeshProUGUI _txtNetworkDebugger;


    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        _btnHost.onClick.AddListener(StartHost);
        _btnClient.onClick.AddListener(StartClient);
        _btnGreen.onClick.AddListener(GreenBet);
        _btnRed.onClick.AddListener(RedBet);

        EventManager.OnChipCountChanged += OnChipCountChanged;

        // DEBUG NETWORKING ------------------------------------------
        Application.logMessageReceived += Application_logMessageReceived;
    }

    // DEBUG NETWORKING ------------------------------------------
    private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
    {
        _txtNetworkDebugger.text += condition + Environment.NewLine;
    }
    // ---------------------------------------------------

    public override void OnDestroy()
    {
        base.OnDestroy();

        _btnHost.onClick.RemoveAllListeners();
        _btnClient.onClick.RemoveAllListeners();
        _btnGreen.onClick.RemoveAllListeners(); ;
        _btnRed.onClick.RemoveAllListeners();
    }

    private void StartHost() => NetworkManager.Singleton.StartHost();
    private void StartClient() => NetworkManager.Singleton.StartClient();

    private void GreenBet() => EventManager.BetMade(ColorBet.Green);
    private void RedBet() => EventManager.BetMade(ColorBet.Red);

    private void OnChipCountChanged(int newChipCount)
    {
        _chipCount.text = newChipCount.ToString();
    }

    public void UpdateTimer(string time)
    {
       if(IsServer)
            UpdateTimerClientRPC(time);
    }

    [ClientRpc]
    private void UpdateTimerClientRPC(string time)
    {
        _timer.text = time;
    }
}
