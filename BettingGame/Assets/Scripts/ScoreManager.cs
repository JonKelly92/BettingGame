using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private const int StartingChips = 100;
    private const int WagerIncrements = 10;

    private int _chipCount;

    private void Awake()
    {
        EventManager.OnAllPlayersConnected += OnAllPlayersConnected;
        EventManager.OnBettingResult += OnBettingResult;
    }

    private void OnDestroy()
    {
        EventManager.OnAllPlayersConnected -= OnAllPlayersConnected;
        EventManager.OnBettingResult -= OnBettingResult;
    }

    private void OnAllPlayersConnected()
    {
        _chipCount = StartingChips;
        UpdateChipCount();
    }

    private void OnBettingResult(BettingResult result)
    {
        if (result == BettingResult.Lose)
        {
            _chipCount -= WagerIncrements;

            if (_chipCount <= 0)
                _chipCount = StartingChips;
        }
        else
            _chipCount += WagerIncrements;

        UpdateChipCount();
    }

    private void UpdateChipCount() => EventManager.ChipCountChanged(_chipCount);
}
