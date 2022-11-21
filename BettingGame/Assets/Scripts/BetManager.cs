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

/// <summary>
/// This class determines whether the player has won or lost the round of betting by comparing
/// the color they bet on with the result (color of the object being bet on)
/// </summary>
public class BetManager : NetworkBehaviour
{
    public static BetManager Instance { get; private set; }

    // This is the color that the player bet on
    private ColorBet _colorBet;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        EventManager.OnBetMade += OnBetMade;

        OnBetColorChanged(ColorBet.None);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        EventManager.OnBetMade -= OnBetMade;
    }

    // the player has chosen a color to bet on
    private void OnBetMade(ColorBet bet)
    {
        OnBetColorChanged(bet);
    }

    // We store the color that was bet on and broadcast that this color has changed
    private void OnBetColorChanged(ColorBet bet)
    {
        _colorBet = bet;
        EventManager.BetColorChanged(_colorBet);
    }

    // We check to see if the player has won or lost by comparing the color they bet on to the color of the object
    public void BetResult(ColorBet bet)
    {
        if (_colorBet == ColorBet.None || bet != _colorBet)
        {       
            EventManager.BettingResult(BettingResult.Lose);
        }
        else
        {
            EventManager.BettingResult(BettingResult.Win);
        }
    }
}
