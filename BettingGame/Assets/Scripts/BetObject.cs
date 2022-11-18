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


        //if (IsServer)
        //    ChangeColorClientRPC(randomNumber);

        if (IsServer)
        {
            int randomNumber = Random.Range(0, 2);

            ChangeColorClientRPC(randomNumber);

            //RandomColorClientRpc((ColorBet)randomNumber);
        }
    }

    //[ClientRpc]
    //private void RandomColorClientRpc(ColorBet bet)
    //{
    //    BetManager.Instance.BetResult(bet);
    //}

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
