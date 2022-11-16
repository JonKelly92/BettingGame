using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class Player : NetworkBehaviour
{
    [SerializeField] private List<Transform> _chipStackLocations = new List<Transform>();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
            transform.SetPositionAndRotation(GameManager.Instance.PlayerOnePosition.position, GameManager.Instance.PlayerOnePosition.rotation);
        else
            transform.SetPositionAndRotation(GameManager.Instance.PlayerTwoPosition.position, GameManager.Instance.PlayerTwoPosition.rotation);
    }
}
