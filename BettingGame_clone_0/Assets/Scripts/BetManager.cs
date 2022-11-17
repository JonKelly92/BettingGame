using Unity.Netcode;

public enum ColorBet
{
    Green = 0,
    Red = 1,
    None = 2,
}

public enum BettingResult
{
    Win = 0,
    Lose = 1
}

public class BetManager : NetworkBehaviour
{
    public static BetManager Instance { get; private set; }

    private NetworkVariable<ColorBet> _colorBet = new NetworkVariable<ColorBet>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        EventManager.OnBetMade += OnBetMade;

        _colorBet.Value = ColorBet.None;
    }

    private void OnBetMade(ColorBet bet)
    {
        if (IsOwner)
            UpdateColorBetServerRPC(bet);
    }

    public void BetResult(ColorBet bet)
    {
        if (bet == ColorBet.None || bet != _colorBet.Value)
            EventManager.BettingResult(BettingResult.Lose);
        else
            EventManager.BettingResult(BettingResult.Win);

        if (IsOwner)
            UpdateColorBetServerRPC(ColorBet.None);
    }

    [ServerRpc]
    private void UpdateColorBetServerRPC(ColorBet bet)
    {
        _colorBet.Value = bet;
    }
}
