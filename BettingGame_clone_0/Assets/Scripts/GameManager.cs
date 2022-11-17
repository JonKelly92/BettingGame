using System;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    private const int MaxPlayers = 2;
    private const int StartingChips = 100;
    private const int TimerLength = 10;
    private const int LosingBetPenalty = 10;


    [SerializeField] private Timer _timer;

    [SerializeField] private Transform _playerOnePosition;
    [SerializeField] private Transform _playerTwoPosition;

    [SerializeField] private GameObject _bettingButtonsPanel;

    //private NetworkVariable<int> _chipCount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private int _chipCount;

    public Transform PlayerOnePosition { get { return _playerOnePosition; } }
    public Transform PlayerTwoPosition { get { return _playerTwoPosition; } }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        EventManager.OnBettingResult += OnBettingResult;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        }

       // _chipCount.OnValueChanged += ChipValueChanged;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        if (IsServer)
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;
    }

    private void OnClientConnectedCallback(ulong obj)
    {
        if (NetworkManager.Singleton.ConnectedClientsList.Count == MaxPlayers)
        {
            ShowButtonPanelClientRPC();
            ChipValueChanged(StartingChips);

            _timer.StartTimer(TimerLength);
        }
    }

    private void OnBettingResult(BettingResult result)
    {
        if (result == BettingResult.Lose)
        {
            if (_chipCount - LosingBetPenalty <= 0)
                ChipValueChanged(StartingChips);
            else
                ChipValueChanged(_chipCount - LosingBetPenalty);
        }

        _timer.StartTimer(TimerLength);
    }

    [ClientRpc]
    private void ShowButtonPanelClientRPC()
    {
        _bettingButtonsPanel.SetActive(true);
    }

    private void ChipValueChanged(int newValue)
    {
        _chipCount = newValue;

        //if(IsOwner)
        //    UpdateChipCountServerRPC(newValue);

        EventManager.ChipCountChanged(newValue);
    }


    //[ServerRpc]
    //private void UpdateChipCountServerRPC (int newValue)
    //{
    //    _chipCount = newValue;
    //}
}
