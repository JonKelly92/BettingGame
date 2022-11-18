using System;

public static class EventManager
{
    public static event Action OnAllPlayersConnected;
    public static event Action<int> OnChipCountChanged;
    public static event Action<ColorBet> OnBetMade;
    public static event Action<ColorBet> OnBetColorChanged; // this is what the player currently has selected as their bet
    public static event Action OnTimerEnded;
    public static event Action<BettingResult> OnBettingResult;
    public static event Action OnSpawnChips;




    public static void AllPlayersConnected() => OnAllPlayersConnected?.Invoke();
    public static void ChipCountChanged(int newChipCount) => OnChipCountChanged?.Invoke(newChipCount);
    public static void BetMade(ColorBet colorBet) => OnBetMade?.Invoke(colorBet);
    public static void BetColorChanged(ColorBet colorBet) => OnBetColorChanged?.Invoke(colorBet);
    public static void TimerEnded() => OnTimerEnded?.Invoke();
    public static void BettingResult(BettingResult result) => OnBettingResult?.Invoke(result);
    public static void SpawnChips() => OnSpawnChips?.Invoke();
}
