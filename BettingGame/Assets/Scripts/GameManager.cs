using System;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    private const int MaxPlayers = 2;
    private const int StartingChips = 100;
    private const int TimerLength = 10;


    [SerializeField] private Timer _timer;

    [SerializeField] private Transform _playerOnePosition;
    [SerializeField] private Transform _playerTwoPosition;

    [SerializeField] private GameObject _bettingButtonsPanel;

    private NetworkVariable<int> _chipCount = new NetworkVariable<int>();

    public Transform PlayerOnePosition { get { return _playerOnePosition; } }
    public Transform PlayerTwoPosition { get { return _playerTwoPosition; } }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        }

        _chipCount.OnValueChanged += ChipValueChanged;
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
            _chipCount.Value = StartingChips;

            _timer.StartTimer(TimerLength);
        }
    }

    private void ChipValueChanged(int previousValue, int newValue)
    {
        EventManager.ChipCountChanged(newValue);
    }

    [ClientRpc]
    private void ShowButtonPanelClientRPC()
    {
        _bettingButtonsPanel.SetActive(true);
    }
}
