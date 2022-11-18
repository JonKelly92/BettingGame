using Unity.Netcode;
using UnityEngine;


public class Player : NetworkBehaviour
{
    [SerializeField] private Renderer _selectedBetColor;

    private readonly NetworkVariable<Color> _netColor = new();

    private void Awake()
    {
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
}
