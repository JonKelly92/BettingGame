using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ChipFactory : NetworkBehaviour
{
    public static ChipFactory Instance { get; private set; }

    [SerializeField] private ChipStack _chipStackPrefab;

    private List<ChipStack> _chipStackList;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public List<ChipStack> CreateChipStacks(List<Transform> chipStackLocations)
    {
        _chipStackList = new List<ChipStack>();
        ChipStack spawnedObject = null;

        for (int i = 0; i < chipStackLocations.Count; i++)
        {
            spawnedObject = Instantiate(_chipStackPrefab);
            spawnedObject.transform.position = chipStackLocations[i].position;

            _chipStackList.Add(spawnedObject);
        }

        return _chipStackList;
    }
}
