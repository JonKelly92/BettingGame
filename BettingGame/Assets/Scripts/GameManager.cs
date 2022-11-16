using System;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Transform _playerOnePosition;
    [SerializeField] private Transform _playerTwoPosition;

    [SerializeField] private GameObject _bettingButtonsPanel;

    private const int MaxPlayers = 2;

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

        if(IsServer)
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
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
            ShowButtonPanelClientRPC();
    }

    [ClientRpc]
    private void ShowButtonPanelClientRPC()
    {
        _bettingButtonsPanel.SetActive(true);
    }
}
