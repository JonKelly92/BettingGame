using UnityEngine;

public class ChipStack : MonoBehaviour
{
    private MeshRenderer[] chipRenderers;

    private void Awake()
    {
        chipRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    private void Start()
    {
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
