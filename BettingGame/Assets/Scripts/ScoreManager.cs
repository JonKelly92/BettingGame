using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Handles the amount of chips the player has and how much they are wagering
/// </summary>
public class ScoreManager : NetworkBehaviour
{
    private const int WagerIncrements = 10;

    private int _currentWager;

    private int _chipCount; // total amount of chips the player has

    private void Awake()
    {
        EventManager.OnAllPlayersConnected += OnAllPlayersConnected;
        EventManager.OnBettingResult += OnBettingResult;
        EventManager.OnBetDecrease += OnBetDecrease;
        EventManager.OnBetIncrease += OnBetIncrease;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        EventManager.OnAllPlayersConnected -= OnAllPlayersConnected;
        EventManager.OnBettingResult -= OnBettingResult;
        EventManager.OnBetDecrease -= OnBetDecrease;
        EventManager.OnBetIncrease -= OnBetIncrease;
    }

    // initialzes the players chips and wager
    private void OnAllPlayersConnected()
    {
        _chipCount = GameManager.StartingChips;
        UpdateChipCount();
        UpdateWagerAmount(WagerIncrements);
    }

    // the player has either won or lost, now update their chips
    private void OnBettingResult(BettingResult result)
    {
        if (result == BettingResult.Lose)
        {
            _chipCount -= _currentWager;

            if (_chipCount <= 0)
                _chipCount = GameManager.StartingChips;
        }
        else
            _chipCount += _currentWager;

        UpdateChipCount();
        UpdateWagerAmount(0);
    }

    // increase the amount of chips being bet
    private void OnBetIncrease()
    {
        if(_currentWager >= _chipCount)
        {
            _currentWager = _chipCount;
            return;
        }

        UpdateWagerAmount(_currentWager + WagerIncrements);
    }

    // decrease the amount of chips being bet
    private void OnBetDecrease()
    {
        if (_currentWager <= 0)
        {
            _currentWager = 0;
            return;
        }

        UpdateWagerAmount(_currentWager - WagerIncrements);
    }

    private void UpdateChipCount()
    {
        // broadcasts the amount of chips the player has
        EventManager.ChipCountChanged(_chipCount);
        // updates the amount of chip stacks on the table to reflect the amount of chips the player has
        EventManager.UpdateChipStacks(_chipCount);
    }

    // updates the amount of chips being wagered
    private void UpdateWagerAmount(int newWager)
    {
        _currentWager = newWager;
        EventManager.WagerAmountChanged(_currentWager);
    }
}
