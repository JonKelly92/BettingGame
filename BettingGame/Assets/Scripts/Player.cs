using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class Player : NetworkBehaviour
{
    [SerializeField] private Renderer _selectedBetColor;

    [SerializeField] private List<Transform> _chipStackLocations = new List<Transform>();

    private List<ChipStack> _chipStackList;

    private readonly NetworkVariable<Color> _netColor = new();

    private void Awake()
    {
        _chipStackList = new List<ChipStack>();

        EventManager.OnBetMade += OnBetMade;

        _netColor.OnValueChanged += OnValueChanged;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        EventManager.OnBetMade -= OnBetMade;
        _netColor.OnValueChanged -= OnValueChanged;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
            transform.SetPositionAndRotation(GameManager.Instance.PlayerOnePosition.position, GameManager.Instance.PlayerOnePosition.rotation);
        else
            transform.SetPositionAndRotation(GameManager.Instance.PlayerTwoPosition.position, GameManager.Instance.PlayerTwoPosition.rotation);

        if (IsOwner)
            SpawnChipsServerRPC();
    }

    private void OnBetMade(ColorBet bet)
    {
        if (!IsOwner)
            return;

        Color betColor = Color.white;

        if (bet == ColorBet.Green)
            betColor = Color.green;
        else
            betColor = Color.red;

        UpdateBetColorServerRpc(betColor);
    }

    private void OnValueChanged(Color previousValue, Color newValue)
    {
        _selectedBetColor.material.color = newValue;
    }

    [ServerRpc]
    private void UpdateBetColorServerRpc(Color color)
    {
        _netColor.Value = color;
    }

    [ServerRpc]
    private void SpawnChipsServerRPC()
    {
        SpawnChipsClientRpc();
    }

    [ClientRpc]
    private void SpawnChipsClientRpc()
    {
        if (IsOwner)
            ChipFactory.Instance.CreateChipStacks(_chipStackLocations);
    }
}
