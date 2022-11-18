using System;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    private const int MaxPlayers = 2;
    private const int TimerLength = 10;

    [SerializeField] private Timer _timer;

    [SerializeField] private Transform _playerOnePosition;
    [SerializeField] private Transform _playerTwoPosition;

    [SerializeField] private GameObject _bettingButtonsPanel;

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
            AllPlayersConnectedClientRPC();
            _timer.StartTimer(TimerLength);
        }
    }

    private void OnBettingResult(BettingResult result)
    {
        _timer.StartTimer(TimerLength);
    }

    [ClientRpc]
    private void AllPlayersConnectedClientRPC()
    {
        _bettingButtonsPanel.SetActive(true);
        EventManager.AllPlayersConnected();
        EventManager.SpawnChips();
    }
}
