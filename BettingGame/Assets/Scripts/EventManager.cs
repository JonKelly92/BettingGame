using System;

public static class EventManager
{
    public static event Action OnAllPlayersConnected; // the maximum number of player allow in the game have all connected to the server
    public static event Action OnTimerEnded; // the round timer has ended, time to determine who won or lost
    public static event Action<int> OnUpdateChipStacks; // used to update the amount of chip stacks on the table, pass in the total amount of chips the player has
    public static event Action OnPlayerReady; // When a player is spawned and they are ready to have their chips spawned
    public static void AllPlayersConnected() => OnAllPlayersConnected?.Invoke();
    public static void TimerEnded() => OnTimerEnded?.Invoke();
    public static void UpdateChipStacks(int amountOfChips) => OnUpdateChipStacks?.Invoke(amountOfChips);
    public static void PlayerReady() => OnPlayerReady?.Invoke();


    // BETTING
    public static event Action<BettingResult> OnBettingResult; // this is broadcasted when it has been determined if the player has won or lost
    public static event Action<ColorBet> OnBetMade; // a bet has been made (red or green was choosen)
    public static event Action<ColorBet> OnBetColorChanged; // this is what the player currently has selected as their bet
    public static event Action OnBetDecrease; // decrease the amount of chips being bet
    public static event Action OnBetIncrease;// increase the amount of chips being bet
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
