using UnityEngine;

/// <summary>
/// This is the ChipStack prefab script
/// It simply changes the color of the stack
/// </summary>
public class ChipStack : MonoBehaviour
{
    private MeshRenderer[] chipRenderers;

    private void Awake()
    {
        chipRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    private void Start()
    {
        SetColor();
    }

    private void SetColor()
    {
        Color color = Random.ColorHSV();

        foreach (var item in chipRenderers)
        {
            item.material.color = color;
        }
    }
}
