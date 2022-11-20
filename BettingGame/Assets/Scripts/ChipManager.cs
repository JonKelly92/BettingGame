using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

public class ChipManager : NetworkBehaviour
{
    [SerializeField] private NetworkObject _chipStackPrefab;
    [SerializeField] private Transform _chipSpawnRefrencePoint;

    private const float MaxStacksPerRow = 5f;
    private const float StackSpacing = 0.1f;

    private List<NetworkObject> _chipStackList;

    private float _stackWidth;


    private void Awake()
    {
        EventManager.OnSpawnChips += OnSpawnChips;

        _chipStackList = new List<NetworkObject>();

        var renderer = _chipStackPrefab.GetComponentInChildren<Renderer>();
        if (renderer == null)
            Debug.LogError("Could not find the Renderer component in the ChipStackPrefab");
        else
            _stackWidth = renderer.bounds.size.x;

        // DEBUG -------------------------------------
        EventManager.OnBetMade += EventManager_OnBetMade;
    }


    //DEBUG ------------------------
    private void EventManager_OnBetMade(ColorBet obj)
    {
        if (IsOwner)
            HideChipsServerRPC(1);
    }
    // ------------------------

    public override void OnDestroy()
    {
        base.OnDestroy();

        EventManager.OnSpawnChips -= OnSpawnChips;
    }

    private void OnSpawnChips()
    {
        if (IsOwner)
            SpawnChipsServerRPC(10);
    }

    [ServerRpc]
    private void SpawnChipsServerRPC(int amountOfStacks)
    {
        int i = 0;
        while (i < amountOfStacks)
        {
            SpawnChips();
            i++;
        }
    }

    [ServerRpc]
    private void HideChipsServerRPC(int amountOfStacks)
    {
        int i = 0;
        while (i < amountOfStacks)
        {
            DespawnChipStack();
            i++;
        }
    }

    private void SpawnChips()
    {
        Vector3 spawnPosition;

        if (_chipStackList.Count == 0)
        {
            // spawn first stack here -> _chipSpawnRefrencePoint
            spawnPosition = _chipSpawnRefrencePoint.position;
        }
        else
        {
            if (_chipStackList.Count % MaxStacksPerRow != 0)
            {
                // we haven't got to the end of this row so lets add another stack
                // get the last stack added to the list and create another stack next to it
                Vector3 position = _chipStackList.Last().transform.position;
                float z = position.z - _stackWidth - StackSpacing;
                spawnPosition = new Vector3(
                    position.x,
                    position.y,
                    position.z - _stackWidth - StackSpacing);
            }
            else
            {
                // we have gotten to the end of the row
                // get the amount of rows so far by dividing the List.Count by MaxStacksPerRow (represented by x)
                // Then get the distance forward we should place the next stack -> (x * _stackWidth) + (x * StackSpacing) = distance to move
                int numberOfRows = (int)(_chipStackList.Count / MaxStacksPerRow);
                float distanceForward = (numberOfRows * _stackWidth) + (numberOfRows * StackSpacing);
                spawnPosition = new Vector3(
                    _chipSpawnRefrencePoint.position.x + distanceForward,
                    _chipSpawnRefrencePoint.position.y,
                    _chipSpawnRefrencePoint.position.z);
            }
        }

        var spawnedObject = Instantiate(_chipStackPrefab);
        spawnedObject.transform.position = spawnPosition;
        spawnedObject.GetComponent<NetworkObject>().Spawn(true);

        _chipStackList.Add(spawnedObject);
    }

    private void DespawnChipStack()
    {
        var stack = _chipStackList.Last();
        _chipStackList.Remove(stack);
        stack.Despawn();
    }
}
