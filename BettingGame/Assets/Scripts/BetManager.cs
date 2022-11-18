using Newtonsoft.Json.Bson;
using System.Diagnostics;
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

    private void OnBetMade(ColorBet bet)
    {
        OnBetColorChanged(bet);
    }

    public void BetResult(ColorBet bet)
    {
        UnityEngine.Debug.Log("Player : " + NetworkManager.Singleton.LocalClientId + ", bet color: " + _colorBet.ToString() + " -- obj color : " + bet.ToString());

        if (_colorBet == ColorBet.None || bet != _colorBet)
        {       
            EventManager.BettingResult(BettingResult.Lose);
        }
        else
        {
            EventManager.BettingResult(BettingResult.Win);
        }

       // OnBetColorChanged(ColorBet.None);
    }

    private void OnBetColorChanged (ColorBet bet)
    {
        _colorBet = bet;
        EventManager.BetColorChanged(_colorBet);
    }
}
