using System;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Manages the flow of the game
/// Handles setting up the game when it starts and the timer which determines when rounds start and end
/// Also contains some important variables 
/// </summary>
public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    public const int StartingChips = 100; // Amount of chips each player start with
    private const int MaxPlayers = 2; // max players allow in the game
    private const int TimerLength = 10; // length of time between rounds of betting

    [SerializeField] private Timer _timer;

    // the players positions at the table
    [SerializeField] private Transform _playerOnePosition;
    [SerializeField] private Transform _playerTwoPosition;

    // this panel contains all the UI elements that the player needs to play the game
    [SerializeField] private GameObject _bettingButtonsPanel;

    // incremented when a player is spawned and ready
    private int _playersSpawned;

    public Transform PlayerOnePosition { get { return _playerOnePosition; } }
    public Transform PlayerTwoPosition { get { return _playerTwoPosition; } }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        EventManager.OnBettingResult += OnBettingResult;
        EventManager.OnPlayerReady += OnPlayerReady;

        _playersSpawned = 0;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        EventManager.OnBettingResult -= OnBettingResult;
        EventManager.OnPlayerReady -= OnPlayerReady;
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

    // the round of betting has ended to the timer restarts and begins the next round
    private void OnBettingResult(BettingResult result)
    {
        _timer.StartTimer(TimerLength);
    }

    // all the clients have connected to the server
    private void OnClientConnectedCallback(ulong obj)
    {
        if (NetworkManager.Singleton.ConnectedClientsList.Count == MaxPlayers)
        {
            AllPlayersConnectedClientRPC();
            _timer.StartTimer(TimerLength);
        }
    }

    [ClientRpc]
    private void AllPlayersConnectedClientRPC()
    {
        _bettingButtonsPanel.SetActive(true);
        EventManager.AllPlayersConnected();
    }

    // When a client is connected, spawned and their character is positioned at the table this is called
    private void OnPlayerReady()
    {
        _playersSpawned++;

        if (_playersSpawned == MaxPlayers)
        {
            SpawnChipsClientRPC();
        }
    }

    // When all players are ready then their chips are spawned
    [ClientRpc]
    private void SpawnChipsClientRPC() => EventManager.UpdateChipStacks(StartingChips);
}
