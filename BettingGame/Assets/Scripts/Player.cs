using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Handles the player's prefab and displays the color they have bet on
/// </summary>
public class Player : NetworkBehaviour
{
    [SerializeField] private Renderer _selectedBetColor;
    [SerializeField] private Transform _chipSpawnRefrencePoint;

    private readonly NetworkVariable<Color> _netColor = new();

    private void Awake()
    {
        EventManager.OnBetColorChanged += OnBetColorChanged;

        _netColor.OnValueChanged += OnValueChanged;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        EventManager.OnBetColorChanged -= OnBetColorChanged;

        _netColor.OnValueChanged -= OnValueChanged;
    }

    // the player prefab has spawned in the game now we need to put it in the correct position at the table
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
            transform.SetPositionAndRotation(GameManager.Instance.PlayerOnePosition.position, GameManager.Instance.PlayerOnePosition.rotation);
        else
        {
            transform.SetPositionAndRotation(GameManager.Instance.PlayerTwoPosition.position, GameManager.Instance.PlayerTwoPosition.rotation);
            // the reference point is moved so the rows of chips flow in the correct direction
            _chipSpawnRefrencePoint.transform.position = new Vector3(
                _chipSpawnRefrencePoint.transform.position.x,
                _chipSpawnRefrencePoint.transform.position.y,
                _chipSpawnRefrencePoint.transform.position.z * -1f);
        }

        if (IsOwner)
            OnPlayerReadyServerRpc();
    }

    // the player's prefab is in the correct position so it is now ok to proceed
    [ServerRpc]
    private void OnPlayerReadyServerRpc()
    {
        EventManager.PlayerReady();
    }

    // the player has chosen a color to bet on
    private void OnBetColorChanged(ColorBet bet)
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

    // assign the new color to the object above the player's head 
    // this displays to all the clients what color this player has bet on
    private void OnValueChanged(Color previousValue, Color newValue)
    {
        _selectedBetColor.material.color = newValue;
    }

    // ask the server to update the color we bet on so all the clients can see it
    [ServerRpc]
    private void UpdateBetColorServerRpc(Color color)
    {
        _netColor.Value = color;
    }
}
