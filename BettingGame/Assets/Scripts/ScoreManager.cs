using Unity.Netcode;
using UnityEngine;

public class ScoreManager : NetworkBehaviour
{
    private const int WagerIncrements = 10;

    private int _currentWager;

    private int _chipCount;

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

    private void OnAllPlayersConnected()
    {
        _chipCount = GameManager.StartingChips;
        UpdateChipCount();
        UpdateWagerAmount(WagerIncrements);
    }

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

    private void OnBetIncrease()
    {
        if(_currentWager >= _chipCount)
        {
            _currentWager = _chipCount;
            return;
        }

        UpdateWagerAmount(_currentWager + WagerIncrements);
    }

    private void OnBetDecrease()
    {
        if (_chipCount <= 0)
        {
            _chipCount = 0;
            return;
        }

        UpdateWagerAmount(_currentWager - WagerIncrements);
    }

    private void UpdateChipCount()
    {
        EventManager.ChipCountChanged(_chipCount);
        EventManager.UpdateChipStacks(_chipCount);

        //if (IsServer)
        //    UpdateChipStacksClientRpc(_chipCount);
    }
    private void UpdateWagerAmount(int newWager)
    {
        _currentWager = newWager;
        EventManager.WagerAmountChanged(_currentWager);
    }

    //[ClientRpc]
    //private void UpdateChipStacksClientRpc(int amountOfChips)
    //{
    //    EventManager.UpdateChipStacks(amountOfChips);
    //}
}
