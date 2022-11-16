using Unity.Netcode;

public enum ColorBet
{
    Green = 0,
    Red = 1,
    None = 2,
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

    private void OnBetMade(ColorBet obj)
    {
        _colorBet.Value = obj;
    }
}
