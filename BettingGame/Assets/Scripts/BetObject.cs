using Unity.Netcode;
using UnityEngine;

/// <summary>
/// This is the object that is being bet on. It randomly changes color (either red or green)
/// </summary>
public class BetObject : NetworkBehaviour
{
    private MeshRenderer _renderer;

    private void Awake()
    {
        EventManager.OnTimerEnded += OnTimerEnded;

        _renderer = GetComponent<MeshRenderer>();
    }

    // The round of betting has ended so a new color is chosen 
    private void OnTimerEnded()
    {
        if (IsServer)
        {
            int randomNumber = Random.Range(0, 2);

            ChangeColorClientRPC(randomNumber);
        }
    }

    // The color of the object is updated and the BetManager is given the new color
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
