using System;
using Unity.Netcode;
using UnityEngine;

public class ChipManager : NetworkBehaviour
{
    [SerializeField] private ChipStack _chipStackPrefab;

    private void Awake()
    {
        EventManager.OnSpawnChips += OnSpawnChips;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        EventManager.OnSpawnChips -= OnSpawnChips;
    }

    private void OnSpawnChips()
    {
        if (!IsOwner) return;

        float x = 0;

        if (transform.position.x > 0)
            x = 2f;
        else
            x = -2f;

        SpawnChipsServerRPC(new Vector3(x, 0, 0));

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

}
