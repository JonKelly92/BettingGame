using System;

public static class EventManager
{
    public static event Action<int> OnChipCountChanged;
    public static void ChipCountChanged(int newChipCount) => OnChipCountChanged?.Invoke(newChipCount);
}
