using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class Player : NetworkBehaviour
{
    [SerializeField] private List<Transform> _chipStackLocations = new List<Transform>();

    private List<ChipStack> _chipStackList;

    private void Awake()
    {
        _chipStackList = new List<ChipStack>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
            transform.SetPositionAndRotation(GameManager.Instance.PlayerOnePosition.position, GameManager.Instance.PlayerOnePosition.rotation);
        else
            transform.SetPositionAndRotation(GameManager.Instance.PlayerTwoPosition.position, GameManager.Instance.PlayerTwoPosition.rotation);

        if (IsOwner)
            SetupChipStacks();
    }

    private void SetupChipStacks()
    {
        SpawnChipsServerRPC();

        for (int i = 0; i < _chipStackList.Count; i++)
        {
            _chipStackList[i].transform.position = _chipStackLocations[i].position;
        }
    }

    [ServerRpc]
    private void SpawnChipsServerRPC()
    {
        _chipStackList = ChipFactory.Instance.CreateChipStacks(10);
    }
}
