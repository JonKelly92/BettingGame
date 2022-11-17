using Unity.Netcode;
using UnityEngine;

public class BetObject : NetworkBehaviour
{
    private MeshRenderer _renderer;

    private void Awake()
    {
        EventManager.OnTimerEnded += OnTimerEnded;

        _renderer = GetComponent<MeshRenderer>();
    }

    private void OnTimerEnded()
    {
        int randomNumber = Random.Range(0, 2);

        if (IsServer)
            ChangeColorClientRPC(randomNumber);
    }

    [ClientRpc]
    private void ChangeColorClientRPC(int randomNumber)
    {
        if (randomNumber == 1)
        {
            _renderer.material.color = Color.red;
            BetManager.Instance.BetResult(ColorBet.Red);
        }
        else
        {
            _renderer.material.color = Color.green;
            BetManager.Instance.BetResult(ColorBet.Green);
        }
    }
}
