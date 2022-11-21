using System;

public static class EventManager
{
    public static event Action OnAllPlayersConnected;
    public static event Action OnTimerEnded;
    public static event Action<int> OnSpawnChips; // The primary use for this is spawn the chips stacks are the start of the game, to update a players chips stacks on the table to match the chips they own use OnUpdateChipStacks
    public static event Action<int> OnUpdateChipStacks; // used to update the amount of chip stacks on the table, pass in the total amount of chips the player has
    public static event Action OnPlayerReady; // When a player is spawned and they are ready to have their chips spawned
    public static void AllPlayersConnected() => OnAllPlayersConnected?.Invoke();
    public static void TimerEnded() => OnTimerEnded?.Invoke();
    public static void SpawnChips(int amountOfChips) => OnSpawnChips?.Invoke(amountOfChips);
    public static void UpdateChipStacks(int amountOfChips) => OnUpdateChipStacks?.Invoke(amountOfChips);
    public static void PlayerReady() => OnPlayerReady?.Invoke();


    // BETTING
    public static event Action<BettingResult> OnBettingResult;
    public static event Action<ColorBet> OnBetMade;
    public static event Action<ColorBet> OnBetColorChanged; // this is what the player currently has selected as their bet
    public static event Action OnBetDecrease;
    public static event Action OnBetIncrease;
    public static event Action<int> OnChipCountChanged; // This passes the updated amount of chips the player has
    public static event Action<int> OnWagerAmountChanged; // This passes the updated amount of chips the player is wagering
    public static void BettingResult(BettingResult result) => OnBettingResult?.Invoke(result);
    public static void BetMade(ColorBet colorBet) => OnBetMade?.Invoke(colorBet);
    public static void BetColorChanged(ColorBet colorBet) => OnBetColorChanged?.Invoke(colorBet);
    public static void BetDecrease() => OnBetDecrease?.Invoke();
    public static void BetIncrease() => OnBetIncrease?.Invoke();
    public static void ChipCountChanged(int newChipCount) => OnChipCountChanged?.Invoke(newChipCount);
    public static void WagerAmountChanged(int wagerAmount) => OnWagerAmountChanged?.Invoke(wagerAmount);






}
