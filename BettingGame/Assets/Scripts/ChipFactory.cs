using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ChipFactory : NetworkBehaviour
{
    //public static ChipFactory Instance { get; private set; }

    [SerializeField] private ChipStack _chipStackPrefab;

    public List<ChipStack> ChipStackList { get; private set; }


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        //if (IsOwner)
        //{
        //    SpawnChipsServerRPC(Vector3.zero);
        //}
    }

    [ServerRpc]
    private void SpawnChipsServerRPC(Vector3 position)
    {
        SpawnChipsClientRpc(position);
    }

    [ClientRpc]
    private void SpawnChipsClientRpc(Vector3 position)
    {
        CreateChipStack(position);
    }

    private void CreateChipStack(Vector3 position)
    {
        var spawnedObject = Instantiate(_chipStackPrefab);
        spawnedObject.transform.position = position;
    }

    //private void Awake()
    //{
    //    if (Instance != null && Instance != this)
    //        Destroy(this);
    //    else
    //        Instance = this;
    //}

    //public List<ChipStack> CreateChipStacks(List<Transform> chipStackLocations)
    //{
    //    ChipStackList = new List<ChipStack>();
    //    ChipStack spawnedObject = null;

    //    for (int i = 0; i < chipStackLocations.Count; i++)
    //    {
    //        spawnedObject = Instantiate(_chipStackPrefab);
    //        spawnedObject.transform.position = chipStackLocations[i].position;

    //        ChipStackList.Add(spawnedObject);
    //    }

    //    return ChipStackList;
    //}
}
