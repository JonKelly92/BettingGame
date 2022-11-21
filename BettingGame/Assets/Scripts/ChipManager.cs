using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// The class spawns and despawns stacks of chips 
/// Spawning and despawning is controlled by the server
/// </summary>
public class ChipManager : NetworkBehaviour
{
    [SerializeField] private NetworkObject _chipStackPrefab;

    // Reference point A is used as the starting point for spawning stacks
    [SerializeField] private Transform _chipSpawnRefrencePoint_A;
    // Reference point B is used to determing the direction to spawn rows of stacks (toward the center of the table)
    [SerializeField] private Transform _chipSpawnRefrencePoint_B;

    private const float MaxStacksPerRow = 5f;
    private const float StackSpacing = 0.1f;

    private List<NetworkObject> _chipStackList;
    // the despawn list is used for object pooling, just grab a stack from here instead of spawning a new one
    private List<NetworkObject> _despawnedChipStackList; 

    // width of the stack prefab, used to determine how far apart the stacks should be spawned
    private float _stackWidth;

    private void Awake()
    {
        EventManager.OnUpdateChipStacks += OnUpdateChipStacks;

        _chipStackList = new List<NetworkObject>();
        _despawnedChipStackList = new List<NetworkObject>();

        var renderer = _chipStackPrefab.GetComponentInChildren<Renderer>();
        if (renderer == null)
            Debug.LogError("Could not find the Renderer component in the ChipStackPrefab");
        else
            _stackWidth = renderer.bounds.size.x;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.OnUpdateChipStacks -= OnUpdateChipStacks;
    }

    // The player has either won or lost chips
    private void OnUpdateChipStacks(int newChipCount)
    {
        if (!IsOwner)
            return;

        UpdateChipsServerRpc(newChipCount, _chipSpawnRefrencePoint_A.position, GetXDirection());
    }

    // Asking the server to update our stacks of chips to match our chip count
    [ServerRpc]
    private void UpdateChipsServerRpc(int newChipCount, Vector3 referencePoint_A, float xDirection)
    {
        int stackCount = newChipCount / 10;

        if (_chipStackList.Count == stackCount)
            return;
        else if (_chipStackList.Count < stackCount)
        {
            // amount of stacks to spawn
            stackCount = stackCount - _chipStackList.Count;
            int i = 0;

            if (_despawnedChipStackList.Count() > 0)
            {
                int spawnCount = 0;

                if (stackCount > _despawnedChipStackList.Count())
                    spawnCount = _despawnedChipStackList.Count();// use all the despawned stacks and then spawn more
                else
                    spawnCount = stackCount; // all the stack we need can come from the despawn list

                stackCount -= _despawnedChipStackList.Count(); // the amount of stacks we need is reduced because we're grabbing them from the despawn list

                NetworkObject stack;

                while (i < spawnCount)
                {
                    // move the despawned stack back to the spawned list and show it to the players
                    stack = _despawnedChipStackList.Last();
                    _despawnedChipStackList.Remove(stack);
                    _chipStackList.Add(stack);
                    stack.gameObject.SetActive(true);
                    stack.Spawn(true);

                    i++;
                }
            }

            i = 0;
            while (i < stackCount)
            {
                SpawnChips(referencePoint_A, xDirection);
                i++;
            }
        }
        else if (_chipStackList.Count > stackCount)
        {
            // amount of stacks to despawn
            stackCount = _chipStackList.Count - stackCount;

            int i = 0;
            while (i < stackCount)
            {
                DespawnChipStack();
                i++;
            }
        }
    }

    // Spawns the chips in rows (a grid like pattern) starting from referencePoint_A moving toward the center of the table
    private void SpawnChips(Vector3 referencePoint_A, float xDirection)
    {
        Vector3 spawnPosition;

        if (_chipStackList.Count == 0)
        {
            // spawn first stack 
            spawnPosition = referencePoint_A;
        }
        else
        {
            if (_chipStackList.Count % MaxStacksPerRow != 0)
            {
                // we have not got to the end of this row so lets add another stack
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
                // we are at the end of the row
                // get the amount of rows so far by dividing the List.Count by MaxStacksPerRow (represented by x)
                // Then get the distance forward we should place the next stack -> (x * _stackWidth) + (x * StackSpacing) = distance to move
                // multiplied by xDirection so the rows move towards the center of the table
                int numberOfRows = (int)(_chipStackList.Count / MaxStacksPerRow);
                float distanceForward = ((numberOfRows * _stackWidth) + (numberOfRows * StackSpacing)) * xDirection;
                spawnPosition = new Vector3(
                    referencePoint_A.x + distanceForward,
                    referencePoint_A.y,
                    referencePoint_A.z);
            }
        }

        var spawnedObject = Instantiate(_chipStackPrefab);
        spawnedObject.transform.position = spawnPosition;
        spawnedObject.GetComponent<NetworkObject>().Spawn(true);

        _chipStackList.Add(spawnedObject);
    }

    private void DespawnChipStack()
    {
        // move the stack from the spawned list to the despawned list and hide it from the players
        var stack = _chipStackList.Last();
        _chipStackList.Remove(stack);
        _despawnedChipStackList.Add(stack);
        stack.gameObject.SetActive(false); 
        stack.Despawn(false);
    }

    // using _chipSpawnRefrencePoint_B and _chipSpawnRefrencePoint_A to determine which direction is the center of the table
    private float GetXDirection()
    {
        Vector3 directionVector = _chipSpawnRefrencePoint_B.position - _chipSpawnRefrencePoint_A.position;
        float xDirection;
        if (directionVector.x > 0)
            xDirection = 1;
        else
            xDirection = -1;

        return xDirection;
    }
}
