using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : NetworkBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private Button _btnHost;
    [SerializeField] private Button _btnClient;

    [SerializeField] private Button _btnGreen;
    [SerializeField] private Button _btnRed;

    [SerializeField] private Button _btnDecreaseBet;
    [SerializeField] private Button _btnIncreaseBet;

    [SerializeField] private TextMeshProUGUI _wagerAmount;
    [SerializeField] private TextMeshProUGUI _chipCount;
    [SerializeField] private TextMeshProUGUI _timer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        _btnHost.onClick.AddListener(StartHost);
        _btnClient.onClick.AddListener(StartClient);
        _btnGreen.onClick.AddListener(GreenBet);
        _btnRed.onClick.AddListener(RedBet);
        _btnDecreaseBet.onClick.AddListener(DecreaseBet);
        _btnIncreaseBet.onClick.AddListener(IncreaseBet);

        EventManager.OnChipCountChanged += OnChipCountChanged;
        EventManager.OnWagerAmountChanged += OnWagerAmountChanged;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        _btnHost.onClick.RemoveAllListeners();
        _btnClient.onClick.RemoveAllListeners();
        _btnGreen.onClick.RemoveAllListeners();
        _btnRed.onClick.RemoveAllListeners();
        _btnDecreaseBet.onClick.RemoveAllListeners();
        _btnIncreaseBet.onClick.RemoveAllListeners();

        EventManager.OnChipCountChanged -= OnChipCountChanged;
        EventManager.OnWagerAmountChanged -= OnWagerAmountChanged;
    }

    private void StartHost() => NetworkManager.Singleton.StartHost();
    private void StartClient() => NetworkManager.Singleton.StartClient();

    private void GreenBet() => EventManager.BetMade(ColorBet.Green);
    private void RedBet() => EventManager.BetMade(ColorBet.Red);

    private void DecreaseBet() => EventManager.BetDecrease();
    private void IncreaseBet() => EventManager.BetIncrease();

    private void OnChipCountChanged(int newChipCount) => _chipCount.text = newChipCount.ToString();
    private void OnWagerAmountChanged(int wagerAmount) => _wagerAmount.text = wagerAmount.ToString();

    public void UpdateTimer(string time)
    {
       if(IsServer)
            UpdateTimerClientRPC(time);
    }

    [ClientRpc]
    private void UpdateTimerClientRPC(string time)
    {
        _timer.text = time;
    }
}
