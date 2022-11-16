using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ChipStack : NetworkBehaviour
{
    private MeshRenderer[] chipRenderers;

    private void Awake()
    {
        chipRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        SetColors();
    }

    private void SetColors()
    {
        Color color = Random.ColorHSV();

        foreach (var item in chipRenderers)
        {
            item.material.color = color;
        }
    }
}
