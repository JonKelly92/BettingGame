using System;

public static class EventManager
{
    public static event Action<int> OnChipCountChanged;
    public static void ChipCountChanged(int newChipCount) => OnChipCountChanged?.Invoke(newChipCount);

    public static event Action<ColorBet> OnBetMade;
    public static void BetMade(ColorBet colorBet) => OnBetMade?.Invoke(colorBet);

    public static event Action OnTimerEnded;
    public static void TimerEnded() => OnTimerEnded?.Invoke();
}
