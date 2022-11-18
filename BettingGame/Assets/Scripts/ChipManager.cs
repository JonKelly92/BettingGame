using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class ChipManager : NetworkBehaviour
{
    [SerializeField] private ChipStack _chipStackPrefab;
    [SerializeField] private Transform _chipSpawnRefrencePoint;

    private const float MaxStacksPerRow = 5f;
    private const float StackSpacing = 0.1f;

    private List<ChipStack> _chipStackList;
    private float _stackWidth;


    private void Awake()
    {
        EventManager.OnSpawnChips += OnSpawnChips;

        _chipStackList = new List<ChipStack>();

        var renderer = _chipStackPrefab.GetComponentInChildren<Renderer>();
        if (renderer == null)
            Debug.LogError("Could not find the Renderer component in the ChipStackPrefab");
        else
            _stackWidth = renderer.bounds.size.x;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        EventManager.OnSpawnChips -= OnSpawnChips;
    }

    private void OnSpawnChips()
    {
        //if (!IsOwner) return;

        //float x = 0;

        //if (transform.position.x > 0)
        //    x = 2f;
        //else
        //    x = -2f;

        //SpawnChipsServerRPC(new Vector3(x, 0, 0));


        for (int i = 0; i < 10; i++)
        {
            PrepareToSpawnChipStack();
        }

    }

    private void PrepareToSpawnChipStack()
    {
        if (!IsOwner) return;

        if (_chipStackList.Count == 0)
        {
           // Debug.Log("FIRST SPAWN");

            // spawn first stack here -> _chipSpawnRefrencePoint
            SpawnChipsServerRPC(_chipSpawnRefrencePoint.localPosition);
        }
        else
        {
            if (_chipStackList.Count % MaxStacksPerRow != 0)
            {
                // we haven't got to the end of this row so lets add another stack
                // get the last stack added to the list and create another stack next to it
                Vector3 position = _chipStackList.Last().transform.localPosition;
                SpawnChipsServerRPC(new Vector3(
                    position.x,
                    position.y, 
                    position.z - _stackWidth - StackSpacing));
            }
            else
            {
                // we have gotten to the end of the row
                // get the amount of rows so far by dividing the List.Count by MaxStacksPerRow (represented by x)
                // Then get the distance forward we should place the next stack -> (x * _stackWidth) + (x * StackSpacing) = distance to move
                int numberOfRows = (int)(_chipStackList.Count / MaxStacksPerRow);
                float distanceForward = (numberOfRows * _stackWidth) + (numberOfRows * StackSpacing);
                SpawnChipsServerRPC(new Vector3(
                    _chipSpawnRefrencePoint.localPosition.x + distanceForward, 
                    _chipSpawnRefrencePoint.localPosition.y, 
                    _chipSpawnRefrencePoint.localPosition.z));
            }
        }
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
        var spawnedObject = Instantiate(_chipStackPrefab, transform);
        spawnedObject.transform.localPosition = position;
        _chipStackList.Add(spawnedObject);
    }
}
