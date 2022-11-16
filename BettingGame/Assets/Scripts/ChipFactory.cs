using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ChipFactory : MonoBehaviour
{
    public static ChipFactory Instance { get; private set; }

    [SerializeField] private ChipStack _chipStackPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public List<ChipStack> CreateChipStacks(int amountOfStacks)
    {
        List<ChipStack> chipStackList = new List<ChipStack>();

        ChipStack spawnedObject = null;

        for (int i = 0; i < amountOfStacks; i++)
        {
            spawnedObject = Instantiate(_chipStackPrefab);
            spawnedObject.GetComponent<NetworkObject>().Spawn(true);
            chipStackList.Add(spawnedObject);
        }

        return chipStackList;
    }
}
